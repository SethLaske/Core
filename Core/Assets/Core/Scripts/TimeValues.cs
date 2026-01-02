namespace Core.Scripts
{
    public class TimeValues
    {
        public float deltaTime { get; private set; }
        public float timeScale { get; private set; }

        public TimeValues(float argDeltaTime, float argTimeScale)
        {
            deltaTime = argDeltaTime;
            timeScale = argTimeScale;
        }
    }
}
