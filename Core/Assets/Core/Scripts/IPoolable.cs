namespace Core.Scripts
{
    public interface IPoolable
    {
        // Called when the object is retrieved from the pool
        void OnSpawn();
        
        // Called when the object is returned to the pool
        void OnDespawn();
    }
}
