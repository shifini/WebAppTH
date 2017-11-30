using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{

    public class EmpleadosController : Controller
    {

        public class ObtenerInstancia
        {
            private static EmpleadoViewModel instance;

            private ObtenerInstancia() { }

            public static EmpleadoViewModel Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new EmpleadoViewModel();
                        instance.Persona = new Persona();
                        instance.Empleado = new Empleado();
                        instance.DatosBancarios = new DatosBancarios();
                        instance.EmpleadoContactoEmergencia = new EmpleadoContactoEmergencia();
                        instance.IndiceOcupacionalModalidadPartida = new IndiceOcupacionalModalidadPartida();
                        instance.PersonaEstudio = new List<PersonaEstudio>();
                        instance.TrayectoriaLaboral = new List<TrayectoriaLaboral>();
                        instance.PersonaDiscapacidad = new List<PersonaDiscapacidad>();
                        instance.PersonaEnfermedad = new List<PersonaEnfermedad>();
                        instance.PersonaSustituto = new PersonaSustituto();
                        instance.DiscapacidadSustituto = new List<DiscapacidadSustituto>();
                        instance.EnfermedadSustituto = new List<EnfermedadSustituto>();
                        instance.EmpleadoFamiliar = new List<EmpleadoFamiliar>();

                    }
                    return instance;
                }

            }
        }

        private readonly IApiServicio apiServicio;


        public EmpleadosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Index()
        {
            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/Empleados/ListarEmpleados");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando estados civiles",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        private async Task CargarCombos()
        {
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "/api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "/api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "/api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "/api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "/api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "/api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "/api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");

            ViewData["IdPaisLugarNacimiento"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdPaisLugarSufragio"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdPaisDireccion"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");

            ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "/api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "/api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");

            ViewData["IdRegimenLaboral"] = new SelectList(await apiServicio.Listar<RegimenLaboral>(new Uri(WebApp.BaseAddress), "/api/RegimenesLaborales/ListarRegimenesLaborales"), "IdRegimenLaboral", "Nombre");
            ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "/api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");
            ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
            ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "/api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
            ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "/api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");


        }

        [HttpPost]
        public async Task<IActionResult> Create(EmpleadoViewModel empleadoViewModel)
        {

            var ins = ObtenerInstancia.Instance;

            ins.Persona = empleadoViewModel.Persona;
            ins.Empleado = empleadoViewModel.Empleado;
            ins.DatosBancarios = empleadoViewModel.DatosBancarios;
            ins.EmpleadoContactoEmergencia = empleadoViewModel.EmpleadoContactoEmergencia;
            ins.IndiceOcupacionalModalidadPartida = empleadoViewModel.IndiceOcupacionalModalidadPartida;
            ins.PersonaSustituto = empleadoViewModel.PersonaSustituto;

            var response = await apiServicio.InsertarAsync(ins, new Uri(WebApp.BaseAddress), "/api/Empleados/InsertarEmpleado");

            if (response.IsSuccess)
            {
                
                LogEntryTranfer logEntryTranfer = new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    ExceptionTrace = null,
                    Message = "Se ha creado un empleado",
                    UserName = "Usuario 1",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                    EntityID = "Empleado",
                    ObjectPrevious = "NULL",
                    ObjectNext = JsonConvert.SerializeObject(response.Resultado),
                };


                var responseLog = await GuardarLogService.SaveLogEntry(logEntryTranfer);

                ViewData["Mensaje"] = Mensaje.Satisfactorio;
                return PartialView("Resultado");
            }
            else
            {
                ViewData["Mensaje"] = Mensaje.Error;
                return PartialView("Resultado");
            }
            ViewData["Error"] = response.Message;
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Create()
        {

            await CargarCombos();

            return View();
        }

        public async Task<JsonResult> ListarNacionalidadIndigena(string etnia)
        {
            var Etnia = new NacionalidadIndigena
            {
                IdEtnia = Convert.ToInt32(etnia),
            };
            var listaNacionalidadIndigena = await apiServicio.Listar<NacionalidadIndigena>(Etnia, new Uri(WebApp.BaseAddress), "api/NacionalidadesIndigenas/ListarNacionalidadesIndigenasPorEtnias");
            return Json(listaNacionalidadIndigena);
        }

        public async Task<JsonResult> ListarCiudadesPorPais(string pais)
        {
            var Pais = new Pais
            {
                IdPais = Convert.ToInt32(pais),
            };
            var listaCiudades = await apiServicio.Listar<Ciudad>(Pais, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorPais");
            return Json(listaCiudades);
        }

        public async Task<JsonResult> ListarProvinciaPorPais(string pais)
        {
            var Pais = new Pais
            {
                IdPais = Convert.ToInt32(pais),
            };
            var listaProvincias = await apiServicio.Listar<Provincia>(Pais, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            return Json(listaProvincias);
        }

        public async Task<JsonResult> ListarCiudadPorProvincia(string provincia)
        {
            var Provincia = new Provincia
            {
                IdProvincia = Convert.ToInt32(provincia),
            };
            var listaCiudades = await apiServicio.Listar<Ciudad>(Provincia, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorProvincia");
            return Json(listaCiudades);
        }

        public async Task<JsonResult> ListarParroquiaPorCiudad(int idCiudad)
        {
            var Ciudad = new Ciudad
            {
                IdCiudad = Convert.ToInt32(idCiudad),
            };
            var listaParroquias = await apiServicio.Listar<Parroquia>(Ciudad, new Uri(WebApp.BaseAddress), "api/Parroquia/ListarParroquiaPorCiudad");
            return Json(listaParroquias);
        }

        public async Task<JsonResult> ListarAreasConocimientosporEstudio(int idEstudio)
        {
            var Estudio = new Estudio
            {
                IdEstudio = Convert.ToInt32(idEstudio),
            };
            var listaAreasConocimientos = await apiServicio.Listar<AreaConocimiento>(Estudio, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosporEstudio");
            return Json(listaAreasConocimientos);
        }

        public async Task<JsonResult> ListarTitulosporAreaConocimiento(int idAreaConocimiento, int idEstudio)
        {
            var Titulo = new Titulo
            {
                IdAreaConocimiento = idAreaConocimiento,
                IdEstudio = idEstudio
            };
            var listaTitulos = await apiServicio.Listar<Titulo>(Titulo, new Uri(WebApp.BaseAddress), "api/Titulos/ListarTitulosporAreaConocimiento");
            return Json(listaTitulos);
        }

        public async Task<JsonResult> InsertarEmpleadoPersona(int idTipoIdentificacion, string Identificacion, string Nombres, string Apellidos, int idSexo, int idGenero, int idEstadoCivil, int idTipoSangre, int idNacionalidad, int etniaF, int nacionalidadIndigenaF, string CorreoPrivado, string FechaNacimiento, string LugarTrabajo, string CallePrincipal, string CalleSecundaria, string Referencia, string Numero, int parroquiaLugarFamiliar, string TelefonoPrivado, string TelefonoCasa, int Parentesco)
        {
            try
            {
                //EmpleadoViewModel empleadoviewmodel = new EmpleadoViewModel();
                var empleadoviewmodel = ObtenerInstancia.Instance;

                empleadoviewmodel.EmpleadoFamiliar.Add(new EmpleadoFamiliar
                {
                    IdParentesco = Parentesco,
                    Persona = new Persona
                    {
                        IdTipoIdentificacion = idTipoIdentificacion,
                        Identificacion = Identificacion,
                        Nombres = Nombres,
                        Apellidos = Apellidos,
                        IdSexo = idSexo,
                        IdGenero = idGenero,
                        IdEstadoCivil = idEstadoCivil,
                        IdTipoSangre = idTipoSangre,
                        IdNacionalidad = idNacionalidad,
                        IdEtnia = etniaF,
                        IdNacionalidadIndigena = nacionalidadIndigenaF,
                        CorreoPrivado = CorreoPrivado,
                        LugarTrabajo = LugarTrabajo,
                        CallePrincipal = CallePrincipal,
                        CalleSecundaria = CalleSecundaria,
                        Referencia = Referencia,
                        Numero = Numero,
                        IdParroquia = parroquiaLugarFamiliar,
                        TelefonoPrivado = TelefonoPrivado,
                        TelefonoCasa = TelefonoCasa
                    }

                }



                );

                return Json(true);
            }
            catch (Exception ex)
            {

                return Json(false);
            }


        }



        public async Task<JsonResult> InsertarFamiliar(int idTipoIdentificacion, string Identificacion, string Nombres, string Apellidos, int idSexo, int idGenero, int idEstadoCivil, int idTipoSangre, int idNacionalidad, int etniaF, int nacionalidadIndigenaF, string CorreoPrivado, DateTime FechaNacimiento, string LugarTrabajo, string CallePrincipal, string CalleSecundaria, string Referencia, string Numero, int parroquiaLugarFamiliar, string TelefonoPrivado, string TelefonoCasa, int Parentesco, string Ocupacion)
        {
            try
            {

                if (!String.IsNullOrEmpty(idTipoIdentificacion.ToString()) && !String.IsNullOrEmpty(Identificacion) && !String.IsNullOrEmpty(Nombres) && !String.IsNullOrEmpty(Apellidos) && !String.IsNullOrEmpty(idSexo.ToString()) && !String.IsNullOrEmpty(idGenero.ToString()) && !String.IsNullOrEmpty(idEstadoCivil.ToString()) && !String.IsNullOrEmpty(idTipoSangre.ToString()) && !String.IsNullOrEmpty(idNacionalidad.ToString()) && !String.IsNullOrEmpty(etniaF.ToString()) && !String.IsNullOrEmpty(nacionalidadIndigenaF.ToString()) && !String.IsNullOrEmpty(CorreoPrivado) && !String.IsNullOrEmpty(FechaNacimiento.ToString()) && !String.IsNullOrEmpty(CallePrincipal) && !String.IsNullOrEmpty(CalleSecundaria) && !String.IsNullOrEmpty(Referencia) && !String.IsNullOrEmpty(Numero) && !String.IsNullOrEmpty(parroquiaLugarFamiliar.ToString()) && !String.IsNullOrEmpty(TelefonoPrivado) && !String.IsNullOrEmpty(TelefonoCasa) && !String.IsNullOrEmpty(Parentesco.ToString()) && !String.IsNullOrEmpty(Ocupacion))
                {
                    
                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.EmpleadoFamiliar.Exists(x => x.Persona.IdTipoIdentificacion == idTipoIdentificacion && x.Persona.Identificacion == Identificacion && x.Persona.Nombres == Nombres && x.Persona.Apellidos == Apellidos);

                    if (!existe)
                    {
                        empleadoviewmodel.EmpleadoFamiliar.Add
                        (new EmpleadoFamiliar
                        {
                            IdParentesco = Parentesco,
                            Persona = new Persona
                            {
                                IdTipoIdentificacion = idTipoIdentificacion,
                                Identificacion = Identificacion,
                                Nombres = Nombres,
                                Apellidos = Apellidos,
                                IdSexo = idSexo,
                                IdGenero = idGenero,
                                IdEstadoCivil = idEstadoCivil,
                                IdTipoSangre = idTipoSangre,
                                IdNacionalidad = idNacionalidad,
                                IdEtnia = etniaF,
                                IdNacionalidadIndigena = nacionalidadIndigenaF,
                                CorreoPrivado = CorreoPrivado,
                                FechaNacimiento = FechaNacimiento,
                                LugarTrabajo = LugarTrabajo,
                                CallePrincipal = CallePrincipal,
                                CalleSecundaria = CalleSecundaria,
                                Referencia = Referencia,
                                Numero = Numero,
                                IdParroquia = parroquiaLugarFamiliar,
                                TelefonoPrivado = TelefonoPrivado,
                                TelefonoCasa = TelefonoCasa,
                                Ocupacion = Ocupacion
                            }
                        }
                        );

                        return Json(true);
                    }
                    else
                    {
                        ViewData["InsertarFamiliar"] = Mensaje.ExisteRegistro;
                        return Json(false);
                    }
                    return Json(false);
                }

            }
            catch (Exception ex)
            {

                return Json(false);
            }

            return Json(false);
        }

        public async Task<JsonResult> EliminarFamiliar(string id)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.EmpleadoFamiliar.Find(c => c.Persona.Identificacion == id);
                empleadoviewmodel.EmpleadoFamiliar.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> InsertarFormacionAcademica(int IdTitulo, string Observaciones, DateTime FechaGraduado, string NoSenescyt)
        {
            try
            {
                  if (!String.IsNullOrEmpty(IdTitulo.ToString()) && !String.IsNullOrEmpty(Observaciones) && !String.IsNullOrEmpty(FechaGraduado.ToString()) && !String.IsNullOrEmpty(NoSenescyt))
                 {
                    
                        var empleadoviewmodel = ObtenerInstancia.Instance;

                        bool existe = empleadoviewmodel.PersonaEstudio.Exists(x => x.IdTitulo == IdTitulo && x.NoSenescyt == NoSenescyt && x.FechaGraduado == FechaGraduado);

                        if (!existe)
                        {
                            empleadoviewmodel.PersonaEstudio.Add(new PersonaEstudio
                            {
                                FechaGraduado = FechaGraduado.Date,
                                Observaciones = Observaciones,
                                IdTitulo = IdTitulo,
                                NoSenescyt = NoSenescyt
                            }
                        );
                            return Json(true);
                        }
                        else
                        {
                            return Json(false);
                        }
                 }

            }
            catch (Exception ex)
            {

                return Json(false);
            }

            return Json(false);
        }


        public async Task<JsonResult> EliminarFormacionAcademica(int idTitulo)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.PersonaEstudio.Find(c => c.IdTitulo == idTitulo);
                empleadoviewmodel.PersonaEstudio.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> InsertarTrayectoriaLaboral(DateTime FechaInicio, DateTime FechaFin, string Empresa, string PuestoTrabajo, string DescripcionFunciones)
        {
            try
                { 

                        if (!String.IsNullOrEmpty(FechaInicio.ToString()) && !String.IsNullOrEmpty(FechaFin.ToString()) && !String.IsNullOrEmpty(Empresa.ToString()) && !String.IsNullOrEmpty(PuestoTrabajo) && !String.IsNullOrEmpty(DescripcionFunciones))
                    {

                        var empleadoviewmodel = ObtenerInstancia.Instance;

                        bool existe = empleadoviewmodel.TrayectoriaLaboral.Exists(x => x.Empresa == Empresa && x.PuestoTrabajo == PuestoTrabajo);
                        if (!existe)
                        {
                            empleadoviewmodel.TrayectoriaLaboral.Add(new TrayectoriaLaboral
                            {
                                FechaInicio = FechaInicio.Date,
                                FechaFin = FechaFin.Date,
                                Empresa = Empresa,
                                PuestoTrabajo = PuestoTrabajo,
                                DescripcionFunciones = DescripcionFunciones

                            }
                            );
                            return Json(true);
                        }
                        else
                        {
                            return Json(false);
                        }
                    }
            }
            catch (Exception ex)
            {

                return Json(false);
            }

            return Json(false);
        }


        public async Task<JsonResult> EliminarTrayectoriaLaboral(string empresa, DateTime fechainicio, DateTime fechafin, string puestotrabajo)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.TrayectoriaLaboral.Find(c => c.Empresa == empresa && c.FechaInicio == fechainicio.Date && c.FechaFin == fechafin.Date && c.PuestoTrabajo == puestotrabajo);
                empleadoviewmodel.TrayectoriaLaboral.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<JsonResult> InsertarDiscapacidad(int IdTipoDiscapacidad, string NumeroCarnet, int Porciento)
        {
            try
            {
                if (!String.IsNullOrEmpty(IdTipoDiscapacidad.ToString()) && !String.IsNullOrEmpty(NumeroCarnet) && !String.IsNullOrEmpty(Porciento.ToString()))
                {
                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.PersonaDiscapacidad.Exists(x => x.IdTipoDiscapacidad == IdTipoDiscapacidad && x.NumeroCarnet == NumeroCarnet && x.Porciento == Porciento);

                    if (!existe)
                    {
                        empleadoviewmodel.PersonaDiscapacidad.Add(new PersonaDiscapacidad
                        {
                            IdTipoDiscapacidad = IdTipoDiscapacidad,
                            NumeroCarnet = NumeroCarnet,
                            Porciento = Porciento,
                        }
                         );

                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(false);
        }


        public async Task<JsonResult> EliminarDiscapacidad(int idTipoDiscapacidad)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.PersonaDiscapacidad.Find(c => c.IdTipoDiscapacidad == idTipoDiscapacidad);
                empleadoviewmodel.PersonaDiscapacidad.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<JsonResult> InsertarEnfermedad(int IdTipoEnfermedad, string InstitucionEmite)
        {
            try
            {
                if (!String.IsNullOrEmpty(IdTipoEnfermedad.ToString()) && !String.IsNullOrEmpty(InstitucionEmite))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.PersonaEnfermedad.Exists(x => x.IdTipoEnfermedad == IdTipoEnfermedad && x.InstitucionEmite == InstitucionEmite);
                    if (!existe)
                    {
                        empleadoviewmodel.PersonaEnfermedad.Add(new PersonaEnfermedad
                        {
                            IdTipoEnfermedad = IdTipoEnfermedad,
                            InstitucionEmite = InstitucionEmite,
                        }
                       );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(false);

        }


        public async Task<JsonResult> EliminarEnfermedad(int idTipoEnfermedad)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.PersonaEnfermedad.Find(c => c.IdTipoEnfermedad == idTipoEnfermedad);
                empleadoviewmodel.PersonaEnfermedad.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> InsertarDiscapacidadSustituto(int IdTipoDiscapacidadSustituto, string NumeroCarnetSustituto, int PorcientoSustituto)
        {
            try
            {
                if (!String.IsNullOrEmpty(IdTipoDiscapacidadSustituto.ToString()) && !String.IsNullOrEmpty(NumeroCarnetSustituto) && !String.IsNullOrEmpty(PorcientoSustituto.ToString()))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.DiscapacidadSustituto.Exists(x => x.IdTipoDiscapacidad == IdTipoDiscapacidadSustituto && x.NumeroCarnet == NumeroCarnetSustituto && x.PorcentajeDiscapacidad == PorcientoSustituto);

                    if (!existe)
                    {
                        empleadoviewmodel.DiscapacidadSustituto.Add(new DiscapacidadSustituto
                        {
                            IdTipoDiscapacidad = IdTipoDiscapacidadSustituto,
                            NumeroCarnet = NumeroCarnetSustituto,
                            PorcentajeDiscapacidad = PorcientoSustituto,
                        }
                        );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }

            return Json(false);
        }


        public async Task<JsonResult> EliminarDiscapacidadSustituto(int idTipoDiscapacidadSustituto)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.DiscapacidadSustituto.Find(c => c.IdTipoDiscapacidad == idTipoDiscapacidadSustituto);
                empleadoviewmodel.DiscapacidadSustituto.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<JsonResult> InsertarEnfermedadSustituto(int IdTipoEnfermedadSustituto, string InstitucionEmiteSustituto)
        {
            try
            {
                if (!string.IsNullOrEmpty(IdTipoEnfermedadSustituto.ToString()) && !string.IsNullOrEmpty(InstitucionEmiteSustituto))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.EnfermedadSustituto.Exists(x => x.IdTipoEnfermedad == IdTipoEnfermedadSustituto && x.InstitucionEmite == InstitucionEmiteSustituto);

                    if (!existe)
                    {
                        empleadoviewmodel.EnfermedadSustituto.Add(new EnfermedadSustituto
                        {
                            IdTipoEnfermedad = IdTipoEnfermedadSustituto,
                            InstitucionEmite = InstitucionEmiteSustituto,
                        }
                        );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(false);
        }


        public async Task<JsonResult> EliminarEnfermedadSustituto(int idTipoEnfermedadSustituto)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.EnfermedadSustituto.Find(c => c.IdTipoEnfermedad == idTipoEnfermedadSustituto);
                empleadoviewmodel.EnfermedadSustituto.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }




        public async Task<JsonResult> ListarRelacionesLaboralesPorRegimen(int regimen)
        {
            try
            {
                var regimenLaboral = new RegimenLaboral
                {
                    IdRegimenLaboral = regimen,
                };
                var listarelacionesLaborales = await apiServicio.Listar<RelacionLaboral>(regimenLaboral, new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaboralesPorRegimen");
                return Json(listarelacionesLaborales);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<JsonResult> ListarModalidadPartidaRelacion(int relacion)
        {
            try
            {
                var relacionLaboral = new RelacionLaboral
                {
                    IdRelacionLaboral = relacion,
                };
                var listaremodalidadPartida = await apiServicio.Listar<ModalidadPartida>(relacionLaboral, new Uri(WebApp.BaseAddress), "api/ModalidadesPartida/ListarModalidadesPartidaPorRelacionLaboral");
                return Json(listaremodalidadPartida);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> ListarTipoNombramientoRelacion(int relacion)
        {
            try
            {
                var relacionLaboral = new RelacionLaboral
                {
                    IdRelacionLaboral = relacion,
                };
                var listarelacionesLaborales = await apiServicio.Listar<TipoNombramiento>(relacionLaboral, new Uri(WebApp.BaseAddress), "api/TiposDeNombramiento/ListarTiposDeNombramientoPorRelacion");
                return Json(listarelacionesLaborales);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        //public async Task<JsonResult> ListarEmpleados()
        //{
        //    var lista = new List<ListaEmpleadoViewModel>();
        //    try
        //    {
        //        lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
        //                                                            , "/api/Empleados/ListarEmpleados");
        //        return Json(lista);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(Mensaje.Error);
        //    }
        //}

        public async Task<IActionResult> ListarEmpleados()
        {

            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/Empleados/ListarEmpleados");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando estados civiles",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }
    }
}