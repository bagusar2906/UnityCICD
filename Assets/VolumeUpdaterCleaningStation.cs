public class VolumeUpdaterCleaningStation : VolumeUpdater
{
    public  double Volume
    {
        get
        {
            var tube = GetComponentInChildren<Tube>();
            if (tube != null)
                return tube.Volume;
            return 0;
        }
        set
        {
            var tube = GetComponentInChildren<Tube>();
            if (tube != null)
                tube.Fill((float)value);
        }
    }
}