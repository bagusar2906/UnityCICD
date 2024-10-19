using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SimulatorClient.DTOs;
using SimulatorClient.Enums;
using SimulatorClient.Handlers;
using SimulatorClient.Protocol;
using UnityEngine;

namespace SimulatorClient
{
    public class PulseServiceHub
    {
        private const int GripperBusId = 4;
        private readonly IDictionary<short, IStationModel> _pulseStationModels;
        private readonly IGantryModel _gantryModel;
        private readonly StatusDisplay _statusDisplay;
        private readonly SetupSelection _setupSelection;
        private readonly Simulator _simulator;
        private readonly SignalR _signalR;

        private readonly string _url;
        private readonly BarcodeScanner _barcodeScanner;

        public PulseServiceHub(IGantryModel gantryModel,
            SetupSelection setupSelection, Simulator simulator)
        {
            _gantryModel = gantryModel;
            _statusDisplay = simulator.laptop.GetComponent<StatusDisplay>();
            _setupSelection = setupSelection;
            _simulator = simulator;
            _barcodeScanner = _simulator.barcodeScanner.GetComponent<BarcodeScanner>();
            _pulseStationModels = new Dictionary<short, IStationModel>();
            // signalRHubURL = Application.absoluteURL;

            _signalR = new SignalR();
            _url = "pulse-hub";
            _signalR.Init(_url);
            

            _signalR.ConnectionStarted += (sender, e) =>
            {
                var status = $"Connected: {e.ConnectionId}";
                UpdateStatus(status);
                RequestInfoFromServer();
            };
            _signalR.ConnectionClosed += (sender, e) =>
            {
                var status = $"Disconnected: {e.ConnectionId}";
                UpdateStatus(status);
            };

            PublishEvents();

            RegisterEvents();
        }

        public Action<RequestInfoDto, IDictionary<short, IStationModel>> OnLoadConfigResponse { private get; set; }

        private void PublishEvents()
        {
            _barcodeScanner.OnDataReceived += (sender, data) =>
            {
                _signalR.Invoke(EventHandlers.OnBarcodeDataReceived, data);
            };
            
            _setupSelection.OnClickImport = setup =>
            {
                var dto = new ProtocolDto()
                {
                    name = setup
                };
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.ImportSetup, json);
            };

            foreach (var motor in _gantryModel.MotorAxis.Values)
            {
                motor.MotorHomeDone += (sender, args) =>
                {
                    var dto = new HomeDoneDto()
                    {
                        busId = args.BusId,
                        motorId = args.MotorID,
                        position = args.Position,
                        stationId = _gantryModel.Id
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.HomeDone, json);
                };

                motor.MotorMoveDone += (sender, args) =>
                {
                    var dto = new MoveDoneDto()
                    {
                        busId = args.BusId,
                        motorId = args.MotorID,
                        status = args.Status,
                        position = args.Position,
                        stationId = _gantryModel.Id
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.MoveDone, json);
                };

                motor.MotorErrorOccured += (sender, args) =>
                {
                    var dto = new MotorErrorDto()
                    {
                        busId = args.BustId,
                        stationId = _gantryModel.Id,
                        motorId = args.MotorID,
                        errorCode = args.MotorErrorCode
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.MotorErrorOccured, json);
                };
            }

            _gantryModel.MotorGripper.OnServoMotionFinished += (sender, args) =>
            {
                var dto = new MoveGripperDoneDto()
                {
                    busId = args.BusId,
                    motorId = (short)args.Id,
                    position = args.Position,
                    stationId = _gantryModel.Id
                };
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.MoveGripperDone, json);
            };

            _gantryModel.MotorGripper.MotorErrorOccured += (sender, args) =>
            {
                var dto = new MotorErrorDto()
                {
                    motorId = args.MotorID,
                    position = args.Position,
                    errorCode = args.MotorErrorCode
                };
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.GripperErrorOccured, json);
            };

            _gantryModel.MotorGripper.OnnHoldingCurrentChanged += (sender, args) =>
            {

                var dto = new CurrentChangedDto()
                {
                    motorId = args.MotorID,
                    holdingCurrent = args.HoldingCurrent
                };
                
                var json = JsonUtility.ToJson(dto);
                _signalR.Invoke(EventHandlers.GripperHoldingCurrentChanged, json);

            };
        }

        private void RegisterMethodProxy(IDictionary<short, IStationModel> stationModels)
        {
            foreach (var stationModel in stationModels.Values)
            {
                var motors = new[] { stationModel.Motor0, stationModel.Motor1 };

                foreach (var motor in motors)
                {
                    motor.MotorMoveDone += (sender, args) =>
                    {
                        var dto = new MoveDoneDto()
                        {
                            busId = stationModel.BusId,
                            motorId = args.MotorID,
                            status = args.Status,
                            position = args.Position,
                            stationId = stationModel.Id
                        };
                        var json = JsonUtility.ToJson(dto);
                        _signalR.Invoke(EventHandlers.MoveDone, json);
                        ;
                    };

                    motor.MotorErrorOccured += (sender, args) =>
                    {
                        var dto = new MotorErrorDto()
                        {
                            busId = stationModel.BusId,
                            stationId = stationModel.Id,
                            motorId = args.MotorID,
                            errorCode = args.MotorErrorCode,
                            position = args.Position
                        };
                        var json = JsonUtility.ToJson(dto);
                        _signalR.Invoke(EventHandlers.MotorErrorOccured, json);
                        var status =
                            $"{EventHandlers.MotorErrorOccured}:[station_{dto.stationId}] Id: {dto.motorId}, error: {dto.errorCode}, pos: {dto.position:F2}";
                        UpdateStatus(status);
                    };

                    motor.OnPositionChanged += (sender, args) =>
                    {
                        var dto = new PositionChangedDto()
                        {
                            busId = stationModel.BusId,
                            motorId = args.MotorID,
                            position = args.Position,
                            stationId = stationModel.Id
                        };
                        var json = JsonUtility.ToJson(dto);
                       // _signalR.Invoke(EventHandlers.PositionChanged, json);
                    };
                }

                stationModel.SliderModel.OnSliderStateChanged += (sender, args) =>
                {
                    var dto = new StateChangedDto()
                    {
                        busId = stationModel.BusId,
                        state = (short)args.State,
                        stationId = stationModel.Id
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.OnSliderStateChanged, json);
                };

                stationModel.ChipClampModel.OnChipClampStateChanged += (sender, args) =>
                {
                    var dto = new StateChangedDto()
                    {
                        busId = stationModel.BusId,
                        state = (short)args.State,
                        stationId = stationModel.Id
                    };
                    var json = JsonUtility.ToJson(dto);
                    _signalR.Invoke(EventHandlers.OnChipClampStateChanged, json);
                };

                foreach (var sensor in stationModel.VolumeSensors)
                {
                    sensor.OnSensorStateChanged += (sender, args) =>
                    {
                        var dto = new VolumeSensorStateDto()
                        {
                            busId = stationModel.BusId,
                            id = args.Id,
                            volume = args.Volume,
                            stationId = stationModel.Id,
                            isTubeAttached = args.IsTubeAttached
                        };
                        var json = JsonUtility.ToJson(dto);
                        _signalR.Invoke(EventHandlers.OnVolumeSensorStateChanged, json);
                    };
                }

                var pulseStation = stationModel as PulseStationModel;
                if (pulseStation == null) continue;
                {
                    foreach (var weightSensor in pulseStation.WeightSensors)
                    {
                        weightSensor.OnSensorValueChanged += (sender, args) =>
                        {
                            var dto = new LoadCellValueDto()
                            {
                                busId = stationModel.BusId,
                                id = args.Id,
                                weight = args.Weight,
                                stationId = stationModel.Id
                            };
                            var json = JsonUtility.ToJson(dto);
                            _signalR.Invoke(EventHandlers.OnWeightChanged, json);
                        };
                    }
                }

            }
        }

        private void RegisterEvents()
        {
            _signalR.On<string>(FunctionHandlers.ScanBarcode, ScanBarcodeAction);
            _signalR.On<string>(FunctionHandlers.MoveAbs, MoveAbsAction);
            _signalR.On<string>(FunctionHandlers.SetMotionAbort, SetMotionAbortAction);
            _signalR.On<string>(FunctionHandlers.MoveGripper, MoveGripperAction);
            _signalR.On<string>(FunctionHandlers.StopMove, StopMoveAction);
            _signalR.On<string>(FunctionHandlers.Home, HomeAction);
            _signalR.On<string>(FunctionHandlers.RequestInfoResponse, ProcessRequestInfoResponse);
            _signalR.On<string>(FunctionHandlers.MoveVel, MoveVelAction);
            _signalR.On<string>(FunctionHandlers.UpdateVolume, UpdateVolumeAction);
            _signalR.On<string>(FunctionHandlers.SliderAction, MoveSliderAction);
            _signalR.On<string>(FunctionHandlers.ChipClampAction, ClampChipAction);
            _signalR.On<string>(FunctionHandlers.ClearMotorFault, ClearMotorFaultAction);
            _signalR.On<string>(FunctionHandlers.ImportSetupResponse, ProcessSetupResponse);
        }

        private void ScanBarcodeAction(string obj)
        {
            _barcodeScanner.StartScan();
        }

        private void ProcessSetupResponse(string response)
        {
            var dto = JsonUtility.FromJson<ProtocolDto>(response);
            _simulator.ClearObjects();
            _simulator.LoadSetup(dto);
        }

        private void ClearMotorFaultAction(string command)
        {
            var dto = JsonUtility.FromJson<ClearMotorFaultDto>(command);
            if (dto.stationId == 0)
            {
                string status;
                if (dto.busId == GripperBusId)
                {
                    _gantryModel.MotorGripper.ClearFault();
                    status = $"[Gantry][BusID:{dto.busId}]{FunctionHandlers.ClearMotorFault}: Gripper: Clear fault";
                }
                else
                {
                    _gantryModel.MotorAxis[dto.busId].ClearFault();
                    status =
                        $"[Gantry][BusID:{dto.busId}]{FunctionHandlers.ClearMotorFault}: Motor Clear fault";
                }

                UpdateStatus(status);
                return;
            }

            switch (dto.motorId)
            {
                case 0:
                    _pulseStationModels[dto.stationId].Motor0.ClearFault();
                    break;
                case 1:
                    _pulseStationModels[dto.stationId].Motor1.ClearFault();
                    break;
            }
        }

        private void SetMotionAbortAction(string command)
        {
            var dto = JsonUtility.FromJson<SetMotionAbortDto>(command);
            string status;
            if (dto.stationId == 0)
            {
                if (dto.busId == GripperBusId)
                {
                    _gantryModel.MotorGripper.MotionAbortEnabled = dto.enableMask > 0;

                    status =
                        $"[Gantry][BusID:{dto.busId}]{FunctionHandlers.SetMotionAbort}: Gripper: Enabled {dto.enableMask}";
                }
                else
                {
                    _gantryModel.MotorAxis[dto.busId].MotionAbortEnabled = dto.enableMask > 0;
                    status =
                        $"[Gantry][BusID:{dto.busId}]{FunctionHandlers.SetMotionAbort}: Motor Enabled {dto.enableMask}";
                }

                UpdateStatus(status);
                return;
            }

            var pulseStation = _pulseStationModels[dto.stationId] as PulseStationModel;
            if (pulseStation != null)
            {
                if (pulseStation.LowVolumeLoadCell != null)
                    _pulseStationModels[dto.stationId].Motor1
                        .SetAbortInputs(new[] { pulseStation.LowVolumeLoadCell });
                _pulseStationModels[dto.stationId].Motor1.MotionAbortEnabled = dto.enableMask > 0;
            }

            status = $"{FunctionHandlers.SetMotionAbort}: Enable: {dto.enableMask}";
            UpdateStatus(status);
        }

        private void ClampChipAction(string command)
        {
            var dto = JsonUtility.FromJson<ClampChipDto>(command);
            _pulseStationModels[dto.stationId].ChipClampModel.Move((ClampState)dto.state);
            var status = $"{FunctionHandlers.ChipClampAction}: State: {dto.state}";
            UpdateStatus(status);
        }

        private void MoveSliderAction(string response)
        {
            var dto = JsonUtility.FromJson<MoveSliderDto>(response);
            _pulseStationModels[dto.stationId].SliderModel.Move((SliderState)dto.state);
            var status = $"{FunctionHandlers.SliderAction}: State: {dto.state}";
            UpdateStatus(status);
        }


        private void MoveVelAction(string response)
        {
            var dto = JsonUtility.FromJson<MoveVelDto>(response);
            switch (dto.motorId)
            {
                case 0:
                    _pulseStationModels[dto.stationId].Motor0.MoveVel(dto.velocity * _simulator.SpeedFactor, dto.isForward);
                    break;
                case 1:
                    _pulseStationModels[dto.stationId].Motor1.MoveVel(dto.velocity * _simulator.SpeedFactor, dto.isForward);
                    break;
                case 6:
                    var cleaningStation = _pulseStationModels[dto.stationId] as CleaningStationModel;
                    if (cleaningStation != null)
                        cleaningStation.Motor3.MoveVel(dto.velocity * _simulator.SpeedFactor, dto.isForward);
                    break;
            }

            var status =
                $"[Pulse_{dto.stationId}]{FunctionHandlers.MoveVel}: Id: {dto.motorId}, vel: {dto.velocity}  fwd: {dto.isForward}";
            UpdateStatus(status);
        }

        private void StopMoveAction(string response)
        {
            var dto = JsonUtility.FromJson<StopMoveDto>(response);
            string status;
            if (dto.stationId == 0)
            {
                _gantryModel.MotorAxis[dto.busId].StopMove();
                status = $"[Gantry][BusID:{dto.busId}]{FunctionHandlers.StopMove}: Id: {dto.motorId}";
                UpdateStatus(status);
                return;
            }

            switch (dto.motorId)
            {
                case 0:
                    _pulseStationModels[dto.stationId].Motor0.StopMove();
                    break;
                case 1:
                    _pulseStationModels[dto.stationId].Motor1.StopMove();
                    break;
                case 6:
                    var cleaningStation = _pulseStationModels[dto.stationId] as CleaningStationModel;
                    if (cleaningStation != null)
                        cleaningStation.Motor3.StopMove();
                    break;
            }

            status = $"[Pulse_{dto.stationId}]{FunctionHandlers.StopMove}: Id: {dto.motorId}";
            UpdateStatus(status);
        }

        private void UpdateVolumeAction(string response)
        {
            var dto = JsonUtility.FromJson<VolumeSensorDto>(response);
            _pulseStationModels[dto.stationId].UpdateVolume(dto);
            var status =
                $"{FunctionHandlers.UpdateVolume}:[station_{dto.stationId}] Id: {dto.id}, Vol: {dto.weight:F2} mL";
            UpdateStatus(status);
        }

        private void VolumeUpdateResponse(VolumeSensorDto dto)
        {
            var json = JsonUtility.ToJson(dto);
            _signalR.Invoke(EventHandlers.VolumeUpdated, json);
        }

        private void HomeAction(string response)
        {
            var dto = JsonUtility.FromJson<HomeDto>(response);
            string status;
            if (dto.stationId == 0)
            {
                _gantryModel.MotorAxis[dto.busId].Home();
                status = $"[Gantry][BusID:{dto.busId}]{FunctionHandlers.Home}: Id: {dto.motorId}";
                UpdateStatus(status);
                return;
            }

            switch (dto.motorId)
            {
                case 0:
                    _pulseStationModels[dto.stationId].Motor0.Home();
                    break;
                case 1:
                    _pulseStationModels[dto.stationId].Motor1.Home();
                    break;
            }

            status = $"[Pulse_{dto.stationId}]{FunctionHandlers.Home}: Id: {dto.motorId}";
            UpdateStatus(status);
        }

        private void MoveAbsAction(string response)
        {
            var dto = JsonUtility.FromJson<MoveAbsDto>(response);
            string status;
            if (dto.stationId == 0)
            {
                _gantryModel.MotorAxis[dto.busId].MoveAbs(dto.position, dto.velocity * _simulator.SpeedFactor);
                status =
                    $"[Gantry][BusID:{dto.busId}]{FunctionHandlers.MoveAbs}: Id: {dto.motorId}, pos: {dto.position:F2}, vel: {dto.velocity}";
                UpdateStatus(status);
                return;
            }

            switch (dto.motorId)
            {
                case 0:
                    _pulseStationModels[dto.stationId].Motor0.MoveAbs(dto.position, dto.velocity * _simulator.SpeedFactor);
                    break;
                case 1:
                    _pulseStationModels[dto.stationId].Motor1.MoveAbs(dto.position, dto.velocity * _simulator.SpeedFactor);
                    break;
            }

            status =
                $"[Pulse_{dto.stationId}][BusID: {dto.busId}]{FunctionHandlers.MoveAbs}: Id: {dto.motorId}, pos: {dto.position:F2}, vel: {dto.velocity}";
            UpdateStatus(status);
        }

        private void MoveGripperAction(string response)
        {
            var dto = JsonUtility.FromJson<MoveGripperDto>(response);
            _gantryModel.MotorGripper.SetServoMotionPosition(dto.position, dto.velocity * _simulator.SpeedFactor);
            var status =
                $"[Gantry]{FunctionHandlers.MoveGripper}: Id: {dto.motorId}, pos: {dto.position:F2}, vel: {dto.velocity}";
            UpdateStatus(status);
        }

        private void RequestInfoFromServer()
        {
            var request = new RequestInfoDto
            {
                message = "Gantry"
            };
            var jsonTxt = JsonUtility.ToJson(request);
            _signalR.Invoke(EventHandlers.RequestInfo, jsonTxt);
        }

        public void Connect()
        {
            try
            {
                UpdateStatus($"Connecting to.{_url}");
                _signalR.Connect();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                UpdateStatus($"Error:{e.Message}");
            }
        }

        private void ProcessRequestInfoResponse(string response)
        {
            var dto = JsonUtility.FromJson<RequestInfoDto>(response);
            if (dto.protocols != null)
            {
                _setupSelection.UpdateSetupList(dto.protocols);
                if (OnLoadConfigResponse != null)
                {
                    OnLoadConfigResponse.Invoke(dto, _pulseStationModels);
                    RegisterMethodProxy(_pulseStationModels);
                }
            }

            UpdateStatus(dto.message);
        }

        private void UpdateStatus(string status)
        {
            Debug.Log(status);
            if (_statusDisplay != null) _statusDisplay.Content.text = status;
        }
    }
}