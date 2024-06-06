using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Windows;
using System;
using wpfOrder.Models;
using wpfOrder.Services;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace wpfOrder.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand PlaceOrderCommand { get; set; }
        public RelayCommand SelectMenuItemCommand { get; set; }


        private int count;
        private int price;
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;RaisePropertyChanged();
            }
        }

        public int Count
        {
            get { return count; }
            set { count = value; RaisePropertyChanged(); }
        }

        private Restaurant restaurant;

        public Restaurant Restaurant
        {
            get { return restaurant; }
            set { restaurant = value; RaisePropertyChanged(); }
        }

        private List<DishMenuItemViewModel> dishMenu;

        public List<DishMenuItemViewModel> DishMenu
        {
            get { return dishMenu; }
            set { dishMenu = value; RaisePropertyChanged(); }
        }

        public MainViewModel()
        {
            this.LoadRestaurant();
            this.LoadDishMenu();

            //PlaceOrderCommand = new RelayCommand(PlaceOrderCommandExecute);
            //SelectMenuItemCommand = new RelayCommand(SelectMenuItemExecute);
            PlaceOrderCommand = new RelayCommand(new Action(PlaceOrderCommandExecute));
            SelectMenuItemCommand = new RelayCommand(new Action(SelectMenuItemExecute));
        }

        private void LoadRestaurant()
        {
            this.Restaurant = new Restaurant();
            this.Restaurant.Name = "周知饭店";
            this.Restaurant.Address = "广东省东莞市大岭山镇怡朗路188号周知饭店";
            this.Restaurant.PhoneNumber = "13588888888 or 0769-6666666";
        }

        private void LoadDishMenu()
        {
            XmlDataService ds = new XmlDataService();
            var dishes = ds.GetDishes();
            this.DishMenu = new List<DishMenuItemViewModel>();
            foreach (var dish in dishes)
            {
                DishMenuItemViewModel item = new DishMenuItemViewModel();
                item.Dish = dish;
                this.DishMenu.Add(item);
            }
        }


        private void PlaceOrderCommandExecute()
        {
            var selectedDishes = this.DishMenu.Where(i => i.IsSelected == true).Select(i => i.Dish.Name).ToList();
            IOrderService orderService = new MockOrderService();
            orderService.PlaceOrder(selectedDishes);
            MessageBox.Show("订餐成功！");
            Price = 0;
        }

        private void SelectMenuItemExecute()
        {
            this.Count = this.DishMenu.Count(i => i.IsSelected == true);
            List <DishMenuItemViewModel> list = this.DishMenu;
            this.Price = 0;//需重置价格，否则会一直累加
            //处理价格
            foreach (DishMenuItemViewModel dish in list)
            {
                if (dish.IsSelected == true)
                {
                    Console.WriteLine(dish.Dish.Price.Substring(0, 2));
                    this.Price += Convert.ToInt32(dish.Dish.Price.Substring(0, 2));
                }
            }
        }
    }

}