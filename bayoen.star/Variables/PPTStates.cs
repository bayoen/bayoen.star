using bayoen.library.General.Enums;

namespace bayoen.star.Variables
{
    public class PPTStates
    {
        public PPTStates()
        {
            this.Main = MainStates.None;
            this.Sub = SubStates.None;
            this.Mode = GameModes.None;

            this.IsEndurance = false;
        }

        public MainStates Main { get; set; }
        public SubStates Sub { get; set; }
        public GameModes Mode { get; set; }
        public bool IsEndurance { get; set; }
    }
}
