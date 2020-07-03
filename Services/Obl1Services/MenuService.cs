using System.Collections.Generic;
using IDataAccess;
using IServices;

namespace Services
{
    public class MenuService : IMenuService
    {
        public IMenuDataAccess menuDataAccess;
        public MenuService(IMenuDataAccess im)
        {
            this.menuDataAccess = im;
        }
        public List<string> GetMenuItems()
        {
            List<string> menuItems = menuDataAccess.GetItems();
            return menuItems;
        }
    }
}
