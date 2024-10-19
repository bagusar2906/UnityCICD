namespace SimulatorClient.Handlers
{
    public static class EventHandlers
    {
        public const string MoveDone = "MoveDone";
        public const string HomeDone = "HomeDone";
        public const string MoveGripperDone  = "MoveGripperDone";
        public const string PositionChanged = "PositionChanged";
        public const string RequestInfo = "RequestInfo";
        public const string OnSliderStateChanged = "OnSliderStateChanged";
        public const string OnChipClampStateChanged = "OnChipClampStateChanged";
        public const string OnWeightChanged = "OnWeightChanged";
        public const string MotorErrorOccured = "MotorErrorOccured";
        public const string GripperErrorOccured = "GripperErrorOccured";
        public const string ImportSetup = "ImportSetup";
        public const string VolumeUpdated = "VolumeUpdated";
        public const string OnVolumeSensorStateChanged = "OnVolumeSensorStateChanged";
        public const string GripperHoldingCurrentChanged = "GripperHoldingCurrentChanged";
        public const string OnBarcodeDataReceived = "OnBarcodeDataReceived";
    }
}