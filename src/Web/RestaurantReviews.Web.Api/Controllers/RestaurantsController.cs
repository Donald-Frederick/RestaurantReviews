﻿using RestaurantReviews.Common;
using RestaurantReviews.Domain;
using RestaurantReviews.Entity;
using RestaurantReviews.Web.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding.Binders;

namespace RestaurantReviews.Web.Api.Controllers
{
    public class RestaurantsController : AuthorizedBaseController
    {
        private IRestaurantRepository _restaurantRepository;

        public RestaurantsController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        /// <summary>
        /// Get list of Restaurants
        /// Can be filtered on city and/or paged
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="cityname"></param>
        /// <returns></returns>
        // GET: api/Restaurants
        //[Route("api/Restaurants/page={page?}/pagesize={pagesize?}/cityname={cityname?}")]
        public Task<IEnumerable<Restaurant>> Get(
            [FromUri(BinderType = typeof(TypeConverterModelBinder))]int? page,
            [FromUri(BinderType = typeof(TypeConverterModelBinder))]int? pagesize,
            [FromUri(BinderType = typeof(TypeConverterModelBinder))]string cityname = null)
        {
            var filter =  string.IsNullOrWhiteSpace(cityname) ? null : new FilterParam() { Field = "City", Operator = OperatorEnum.Equal, Value = cityname }.ToDbFilter<Restaurant>();
            return _restaurantRepository.GetRestaurantsAsync(page??1, pagesize??1000,  filter);
        }

        /// <summary>
        /// Search restaurants with a filter and/or paged
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        // POST: api/Restaurants/Searches?page=1&pagesize=20   body-{Field:"Name", Operator:"Like", Value:"Pitts%"}
        [Route("api/Restaurants/Searches")]
        public Task<IEnumerable<Restaurant>> Post([FromBody]FilterParam filter,
            [FromUri(BinderType = typeof(TypeConverterModelBinder))] int? page,
            [FromUri(BinderType = typeof(TypeConverterModelBinder))] int? pagesize )
        {
            return _restaurantRepository.GetRestaurantsAsync(page??1, pagesize??1000, filter?.ToDbFilter<Restaurant>());
        }

        /// <summary>
        /// Get a restaurant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Restaurants/5
        public Task<Restaurant> Get(int id)
        {
            return _restaurantRepository.GetRestaurantAsync(id);
        }

        /// <summary>
        /// Create a restaurant
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST: api/Restaurants
        public Task<Restaurant> Post([FromBody] Restaurant value)
        {
            return _restaurantRepository.CreateRestaurantAsync(value.Name, value.Address, value.City);
        }
        
        /// <summary>
        /// delete a restaurant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Restaurants/5
        public Task Delete(int id)
        {
            return _restaurantRepository.DeleteRestaurantAsync(id);
        }
    }
}
