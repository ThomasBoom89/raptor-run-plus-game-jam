namespace RaptorRunPlus.scene.service;

public class DIContainer
{
    private static Factory _factory = null;

    public static Factory GetFactory()
    {
        if (_factory == null)
        {
            _factory = new Factory();
        }

        return _factory;
    }
}