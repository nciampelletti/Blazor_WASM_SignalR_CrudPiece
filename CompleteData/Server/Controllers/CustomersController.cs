using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompleteData.Shared.Models;
using CompleteData.Server.Data;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompleteData.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        RepositoryEF<Customers, NorthwindContext> _customersManager;

        public CustomersController(RepositoryEF<Customers, NorthwindContext> customersManager)
        {
            _customersManager = customersManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Customers>>> GetAllCustomers()
        {
            try
            {
                var result = await _customersManager.GetAll();
                return Ok(new APIListOfEntityResponse<Customers>()
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                // log exception here
                return StatusCode(500);
            }
        }

        [HttpGet("{CustomerId}")]
        public async Task<ActionResult<APIEntityResponse<Customers>>> GetByCustomerId(string CustomerId)
        {
            try
            {
                var result = (await _customersManager.Get(x => x.CustomerId == CustomerId)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Customers>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Customers>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Customer Not Found" },
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // log exception here
                return StatusCode(500);
            }
        }

        [HttpGet("{ContactName}/searchbycontactname")]
        public async Task<ActionResult<APIListOfEntityResponse<Customers>>> SearchByContactName(string ContactName)
        {
            try
            {
                var result = await _customersManager.Get(x => x.ContactName.ToLower().Contains(ContactName.ToLower()));
                if (result != null && result.Count() > 0)
                {
                    return Ok(new APIListOfEntityResponse<Customers>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Customers>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Customers Not Found" },
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // log exception here
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIEntityResponse<Customers>>> Post([FromBody] Customers Customer)
        {
            try
            {
                await _customersManager.Insert(Customer);
                var result = (await _customersManager.Get(x => x.CustomerId == Customer.CustomerId)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Customers>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Customers>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find customer after adding it." },
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // log exception here
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIEntityResponse<Customers>>> Put([FromBody] Customers Customer)
        {
            try
            {
                await _customersManager.Update(Customer);
                var result = (await _customersManager.Get(x => x.CustomerId == Customer.CustomerId)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Customers>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Customers>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find customer after updating it." },
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // log exception here
                return StatusCode(500);
            }
        }

        [HttpDelete("{CustomerId}")]
        public async Task<ActionResult> Delete(string CustomerId)
        {
            try
            {
                var CustomerList = await _customersManager.Get(x => x.CustomerId == CustomerId);
                if (CustomerList != null)
                {
                    var Customer = CustomerList.First();
                    var success = await _customersManager.Delete(Customer);
                    if (success)
                        return NoContent();
                    else
                        return StatusCode(500);
                }
                else
                    return StatusCode(500);
            }
            catch (Exception ex)
            {
                // log exception here
                var msg = ex.Message;
                return StatusCode(500);
            }
        }
    }
}
