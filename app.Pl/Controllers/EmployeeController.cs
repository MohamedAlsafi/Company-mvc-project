using app.BLL.Interface;
using app.BLL.Repository;
using app.DAL.model;
using app.Pl.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Pl.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        //private readonly IEmployeeReopsitory _employeeReopsitory;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            //_employeeReopsitory = employeeReopsitory;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task <IActionResult> Index(string SearchName)
        {
            IEnumerable<Employee> employess;
            if (string.IsNullOrEmpty(SearchName)) 
            {
                 employess = await _unitOfWork.employeeReopsitory.GetAll();
                
            }
            else
            {
               employess=  _unitOfWork.employeeReopsitory.GetEmpolyeeByName(SearchName);
            }

                var MapedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employess);
                return View(MapedEmp);
        }
        [HttpGet]
        public  IActionResult Create()
        {
            //ViewBag.Departments = await _unitOfWork.departmentRepository.GetAll();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(EmployeeViewModel employeeVM )
        {
            if(ModelState.IsValid)
            {
                ///manual mapping 
                ///var mapedEmployee = new Employee()
                ///{
                ///    Name = employeeVM.Name,
                ///    Age = employeeVM.Age,
                ///    PhoneNumbers = employeeVM.PhoneNumbers
                ///};

                var mapedEmp = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
               await _unitOfWork.employeeReopsitory.Add(mapedEmp);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);

        }
        public async Task <IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee = await _unitOfWork.employeeReopsitory.Get(id.Value);
            if (employee is null)
                return NotFound();
                var mapedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
            
            return View(ViewName, mapedEmp);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments = await _unitOfWork.departmentRepository.GetAll();


            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mapedEmp =_mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                    _unitOfWork.employeeReopsitory.Update(mapedEmp);
                   await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVM);
        }
        [HttpGet]
        public async Task <IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVm)
        {
            if (id != employeeVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mapedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.employeeReopsitory.Delete(mapedEmp);
                   await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVm);
        }
    }
}
