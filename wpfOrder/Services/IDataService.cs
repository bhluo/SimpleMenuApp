using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfOrder.Models;

namespace wpfOrder.Services
{
    public interface IDataService
    {
        List<Dish> GetDishes();
    }
}
