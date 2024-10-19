using System.Collections;
using SimulatorClient;
using UnityEngine;
using UnityEngine.UI;

public class FindReference : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly float _searchingDistance = 1500;
    private readonly float _searchingSpeed = 5;


    public void StartSearch()
    {
        var list = GetComponentInChildren<Dropdown>();
        print(list.options[ list.value].text);
        StartCoroutine(Search(list.options[ list.value].text, _searchingDistance));
        
    }
    private IEnumerator Search(string stationType, float searchingDistance)
    {
        var simulator = Simulator.Instance;
        
        //  simulator.MoveXY(startingPoint.x, startingPoint.y, 500);
        // yield return simulator.WaitXYMovement();
        var  startingPoint = simulator.CurrentPosition;
     
        var abortSensor = Laser.Instance;
        var position = new Vector2();
        abortSensor.Activated = true;
        IMotorSim motor;
        
        if (stationType.StartsWith("Deck"))
        {
            print("Searching for Top Right Deck reference ...");
            
            abortSensor.Clear();
            abortSensor.StopThreshold = 2;
            motor = XAxisMotor.Instance;
            simulator.SetMotionAbort(motor, abortSensor, true);
            simulator.MoveAbs(Axis.X, startingPoint.x + searchingDistance, _searchingSpeed);
            yield return simulator.WaitMove(new[] { Axis.X });

            if (abortSensor.IsAbortTriggered)
            {
                simulator.SetMotionAbort(motor, abortSensor, false);
                position.x = (float)motor.CurrentPos;
                print($"Found  x pos: {position.x:F2}");
                simulator.MoveAbs(Axis.X, (float)(position.x - 10f), 200f); // move slightly left
                yield return simulator.WaitMove(new[] { Axis.X });
            }
            else
            {
                simulator.SetMotionAbort(motor, abortSensor, false);
                print($"Reference not found");
                yield break;
            }

            print($"Search in y axis ..");
            motor = YAxisMotor.Instance;
            simulator.SetMotionAbort(motor, abortSensor, true);
            simulator.MoveAbs(Axis.Y, startingPoint.y + searchingDistance, _searchingSpeed);
            yield return simulator.WaitMove(new[] { Axis.Y });
            

            if (abortSensor.IsAbortTriggered)
            {
                position.y = (float)motor.CurrentPos;
                print($"Found  y pos: {position.y:F2}");
                print($"Moving to top right position ...({position.x:F2},{position.y:F2})");
                simulator.MoveXY( position.x, position.y , 40f);
                yield return simulator.WaitMove(new[] { Axis.X, Axis.Y });
                print($"Top Right position: ({position.x:F2},{position.y:F2})");
            }

            simulator.SetMotionAbort(motor, abortSensor, false);
        }
        else
        {
            print("Searching for Tube Rack A1 reference ...");
            
            print("Searching for first edge reference ...");
            abortSensor.StopThreshold = 2;
            motor = XAxisMotor.Instance;
            simulator.SetMotionAbort(motor, abortSensor, true);
            simulator.MoveAbs(Axis.X, startingPoint.x - searchingDistance, _searchingSpeed);
            yield return simulator.WaitMove(new[] { Axis.X });
            //yield return new WaitUntil(() => motor.State is not (MotorState.Started or MotorState.Moving));

            
            if (abortSensor.IsAbortTriggered)
            {
                var edge1 = (float)motor.CurrentPos;
                print($"Found  x1 pos: {edge1:F2}");
                simulator.SetMotionAbort(motor, abortSensor, false);
                simulator.MoveAbs(Axis.X, (float)(motor.CurrentPos + 2d), _searchingSpeed);
                yield return simulator.WaitMove(new[] { Axis.X });
                
                print("Searching for x2 ...");
                simulator.SetMotionAbort(motor, abortSensor, true);
                simulator.MoveAbs(Axis.X, (float)(motor.CurrentPos + searchingDistance), _searchingSpeed);
                yield return simulator.WaitMove(new[] { Axis.X });
                var edge2 = (float)motor.CurrentPos;
                
                print($"Found  x2 pos: {edge2:F2}");
                simulator.SetMotionAbort(motor, abortSensor, false);
                
                var start2NdSearch = edge1 + (edge2 - edge1) / 2;
                
                print($"Positioning to x center: {start2NdSearch:F2}  for finding y pos");
                simulator.MoveAbs(Axis.X, start2NdSearch, _searchingSpeed);
                yield return simulator.WaitMove(new[] { Axis.X });
                
                var centerX = motor.CurrentPos;
                motor = YAxisMotor.Instance;
                print("Searching for y1 ...");
                simulator.SetMotionAbort(motor, abortSensor, true);
                simulator.MoveAbs(Axis.Y, (float)(motor.CurrentPos + searchingDistance), _searchingSpeed);
                yield return simulator.WaitMove(new[] { Axis.Y });
                edge1 = (float)motor.CurrentPos;
                simulator.SetMotionAbort(motor, abortSensor, false);
                print($"Found  y1 pos: {edge1:F2}");
                
                //slightly move
                simulator.MoveAbs(Axis.Y, (float)(motor.CurrentPos - 2), 20f);
                yield return simulator.WaitMove(new[] { Axis.Y });
                
                print("Searching for y2 position ...");
                simulator.SetMotionAbort(motor, abortSensor, true);
                simulator.MoveAbs(Axis.Y, (float)(motor.CurrentPos - searchingDistance), _searchingSpeed);
                yield return simulator.WaitMove(new[] { Axis.Y });
                edge2 = (float)motor.CurrentPos;
                simulator.SetMotionAbort(motor, abortSensor, false);
                print($"Found  y2 pos: {edge2:F2}");
                
                var centerY = edge2 + (edge1 - edge2)/2;
                print($"Positioning to center...: ( {centerX:F2}, {centerY:F2} )");
                simulator.MoveAbs(Axis.Y, centerY, 20f);
                yield return simulator.WaitMove(new[] { Axis.Y });
                print($"Center position...: ( {centerX:F2}, {centerY:F2} )");
                
            }

        }
    }

   
}
