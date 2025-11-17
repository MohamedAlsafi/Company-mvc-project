using app.BLL.Interface;
using app.DAL.model;
using app.Pl.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Pl.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
	{
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper )
        {
			//_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task <IActionResult> Index()
		{
			var Departments = await _unitOfWork.departmentRepository.GetAll();
			var MapedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(Departments);
			return View(MapedDepartment);
		}
		[HttpGet]
		public IActionResult Create() 
		{ 
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(DepartmentViewModel departmentVM)
        {
			if(ModelState.IsValid) // server side validation 
			{
				var MapedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);	

              await  _unitOfWork.departmentRepository.Add(MapedDepartment);
				await _unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
			}
            return View(departmentVM);
        }
		public async Task <IActionResult> Details (int? id , string ViewName= "Details")
		{
			if (id is null)
			{
				return BadRequest();
			}
			var department = await _unitOfWork.departmentRepository.Get(id.Value);
			if (department is null)
				return NotFound();
            var MapedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);


            return View( ViewName, MapedDepartment);
		}
		[HttpGet]
		public async Task <IActionResult> Edit(int? id)
		{
			///if (id is null)
			///	return BadRequest();
			///var department = _unitOfWork.employeeReopsitory.Get(id.Value);
			///if (department is null)
			///	return NotFound();
			///return View(department);
			return await Details(id, "Edit");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task <IActionResult> Edit ([FromRoute]int id, DepartmentViewModel departmentVM)
		{
			if (id != departmentVM.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try 
				{
                    var MapedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                    _unitOfWork.departmentRepository.Update(MapedDepartment);
					await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
				catch (Exception ex)
				{
					ModelState.AddModelError(string .Empty, ex.Message);
				}
				
			}
			return View(departmentVM);
		}
        [HttpGet]
        public async Task <IActionResult> Delete(int? id)
        {
            
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete ([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MapedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                    _unitOfWork.departmentRepository.Delete(MapedDepartment);
					await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(departmentVM);
        }
    }
}
