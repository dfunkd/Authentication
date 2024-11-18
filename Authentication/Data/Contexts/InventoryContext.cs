namespace Authentication.Data.Contexts;

public interface IInventoryContext : IContext;

public class InventoryContext(IConfiguration configuration) : BaseContext(configuration.GetConnectionString("InventoryDb")), IInventoryContext;
