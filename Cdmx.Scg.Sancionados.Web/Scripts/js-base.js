/*! version : 0.1
 =========================================================
 js-base.js
 2020 Rigoberto Nava
 =========================================================
 */
(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory(require('jquery'), require('axios'), require('toastr')) :
        typeof define === 'function' && define.amd ? define(['jquery'], ['axios'], ['toastr'], factory) :
            (global = global || self, global.JsBase = factory(global.jQuery, global.axios, global.toastr));
}(this, (function ($, axios, toastr) {
    'use strict';

    $ = $ && $.hasOwnProperty('default') ? $['default'] : $;
    axios = axios && axios.hasOwnProperty('default') ? axios['default'] : axios;
    toastr = toastr && toastr.hasOwnProperty('default') ? toastr['default'] : toastr;


////; (function (root, factory) {

////    if (typeof define === 'function' && define.amd) {
////        define(factory);
////    } else if (typeof exports === 'object') {
////        module.exports = factory();
////    } else {
////        root.JsBase = factory();
////    }

////})(this, function () {
////    'use strict';

    var JsBase = {};

    
    /**
    * Define el encabezado para las solictudes ajax de axios
    */
    axios.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

    /**
     * Nombre de la libreria
     */
    JsBase.nombre = 'jsBase';

    /**
     * Valores por defecto al inicializar la libreria
     * 
     * @Autor:  ngr <2020/05/14>
     * @Modif:  xxx <>
     */
    var Default = JsBase.default = {
        classOpenSideBar: 'settings-open',
        classDivSideBar: 'ui-theme-settings',
        classDivContainerSideBar: 'contenedor-barra-derecha',
        mensajeEliminar: '¿Esta seguro(a) de eliminar el registro?',
        mensajeConfirmacion: '¿Esta seguro(a) de realizar la acción?'
    }

    
    /**
     * Modifica la configuracion por defecto durante el uso de la libreria.
     * @param {object} [opciones] - Informacion de defecto.
     * @param {string} [opciones.classOpenSideBar] - Nobre de la clase que tiene asignada el contenedor que muestra la barra lateral derecha.
     * @param {string} [opciones.classDivSideBar] - Nomvre de la clase que tiene asignada el contedor de la barra lateral derecha.
     * @param {string} [opciones.classDivContainerSideBar] - Nombre de la clase que tiene asignado el contenedor donde sera integrador el html de los formularios
     *
     * @Uso:    jsBase.configure({classOpenSideBar: 'sidebar-open-otro'});
     * @Nota:   Puede modificar una o varias configuraciones
     * @Autor:  ngr <2020/05/14>
     * @Modif:  xxx <>
     */
    JsBase.configurar = (opciones) => {
        var key, value;
        for (key in opciones) {
            value = opciones[key];
            if (value !== undefined && options.hasOwnProperty(key)) Default[key] = value;
        }

        return JsBase;
    }

    /**
     * Agrega el evento para cargar la vista de editar o crear un registro.
     * @param {string} [strClaseAgregarForm] - Nombre de la clase que identifica el objeto que desencadena el evento para agregar un formulario ejemplo class="agregar-form-nuevo"
     *
     * @Uso:    jsBase.agregarFormBarraLateral('agregar-form-nuevo');
     * @Nota:   En el boton de "Nuevo" agregar la clase class="agregar-form-nuevo", para "Editar" seria class="agregar-form-editar" en el link de edicion.
     *          El div o boton debe contener los siguientes atributos
     *              (obligatorio) data-url="/Controlador/Accion/Id" Donde el valor es la ruta del controlador con su accion y su id
     *              (opcional) data-container="contenedor-formulario" Donde el valor es la clase de div donde vaciara el contenido html, se encuentra en el index
     * @Autor:  ngr <2020/05/14>
     * @Modif:  xxx <>
    */
    JsBase.agregarFormBarraLateral = (strClaseAgregarForm) => {

        //OBTIENE EL OBJETO QUE TIENE LA CLASE form-edit
        $('.' + strClaseAgregarForm).click(function (e) {

            // Valida que el contendor contenga el atributo data url
            if (typeof $(this).data('url') === 'string' || $(this).data('url') instanceof String) {

                //Obtiene la url del atributo data-url
                var strUrl = $(this).data('url');

                //Obtiene el contenedor donde cargara el formulario para crear o editar
                //lo obtiene del atributo data-container
                var divContenedor = $(this).data('container') || Default.classDivContainerSideBar;

                //Inicializa la barra de progreso
                NProgress.start();

                // Hace la peticion via get mediante axios
                axios.get(strUrl)
                    .then(function (respuesta) {

                        //Asigna el formulario de respuesta al contenedor
                        $('.' + divContenedor).html(respuesta.data);

                        //Valida si no contiene la clase que muestra la barra lateral derecha, 
                        //en caso de estar oculta entonces la muestra
                        if (!$('.' + Default.classDivSideBar).hasClass(Default.classOpenSideBar))
                            $('.' + Default.classDivSideBar).toggleClass(Default.classOpenSideBar);

                    })
                    .catch(function (errormessage) {
                        console.log(errormessage);
                        //Si existe un error entonces despliega los datos en una notificacion
                        if (errormessage.isAxiosError)
                            toastr.error('Existe un error, contacte al administrador: ' + errormessage.message);
                        else
                            toastr.error('Existe un error, contacte al administrador');
                    })
                    .then(function () {
                        NProgress.done();
                    });

            }
            else {
                toastr.warning("No se definio la URL para cargar el formulario");
            }


            //if ($(this).data('confirm') != null && $(this).data('confirm') != 'undefined' && $(this).data('confirm') != '') {
            //    blnRespuesta = false;
            //}
            //else {


            //}


        });

        return JsBase;
    }//agregarFormBarraLateral

    /**
     * Agrega el evento para cargar una vista a partir de una URL.
     * @param {string} [strUrl] - URL de la vista
     *
     * @Uso:    jsBase.agregarFormBarraLateralURL('Editar/5');
     * @Nota:   Puede moficiar la configuracion por defecto para modificar el contenedor del sidebar
     * @Autor:  ngr <2020/05/14>
     * @Modif:  xxx <>
    */
    JsBase.agregarFormBarraLateralURL = (strUrl) => {

        //Inicializa la barra de progreso
        NProgress.start();

        // Optionally the request above could also be done as
        axios.get(strUrl)
            .then(function (respuesta) {
                
                //Asigna los datos en el div
                $('.' + Default.classDivContainerSideBar).html(respuesta.data);

                //Valida si no contiene la clase que muestra la barra lateral derecha, 
                //en caso de estar oculta entonces la muestra
                if (!$('.' + Default.classDivSideBar).hasClass(Default.classOpenSideBar))
                    $('.' + Default.classDivSideBar).toggleClass(Default.classOpenSideBar);
            })
            .catch(function (errormessage) {
                console.log(errormessage);
                //Si existe un error entonces despliega los datos en una notificacion
                if (errormessage.isAxiosError)
                    toastr.error('Existe un error, contacte al administrador: ' + errormessage.message);
                else
                    toastr.error('Existe un error, contacte al administrador');
            })
            .then(function () {
                NProgress.done();
            });

        return JsBase;

    }//agregarFormBarraLateralURL



    /**
     * Agrega el evento para enviar la forma y que registre o actualice la informacion en la base de datos
     * @param {string} [strClaseEnviarForm] - Nombre de la clase que identifica el objeto que desencadena el evento para enviar un formulario
     * @param {boolean} [bolRecargaGrid] - Indica si debe recargar el grid  
     * @param {string} [strNombreGrid] - Nombre de grid que se va recargar
     *
     * @Uso:     JsBase.EnviarFormulario('form-enviar', true, 'tabla-principal');
     * @Nota:    Si no se modifica la configuracion, entonces tomara por defecto la opcion "opciones.classOpenSideBar" para cerrar el side bar
     *               ya sea al cancelar, o al terminar de enviar el formulario
     *           En el tag del formulario (form) agregar la clase
     *               class="form-crear", dicho nombre es con que se buscara el formulario
     *               data-url="/Controlador/Accion"
     *              (opcional) data-confirm="mensaje de confirmacion" Donde el valor es el mensaje que se desea desplegar en la confirmacion
     *                                                                Si encuentra el data confirm entonces muestra el mensaje de lo contrario cargara el formulario
     * @Autor:   ngr <2020/05/15>
     * @Modif:   xxx <>
    */
    JsBase.enviarFormulario = (strClaseEnviarForm, bolRecargaGrid, strNombreGrid) => {

        //Inicializar la Validacion
        $.validator.unobtrusive.parse($('.' + strClaseEnviarForm));

        //Agrega la funcionalidad al formulario en el evento submit para enviar el formulario
        $('.' + strClaseEnviarForm).on("submit", function (event) {

            //Variable para obtener la respuesta del data-confirm en caso de que el objeto lo tenga
            var blnRespuesta = true;

            //Deshabilita el envio de la forma
            event.preventDefault();     

            //Activa el validador de los campos
            $.validator.unobtrusive.parse($(this));

            //Valida si los campos estan correctos
            if ($(this).valid()) {

                //Si el objeto tiene un data-confirm, mostrar el mensaje y permitir confirmar al usuario
                if ($(this).data('confirm') != null && $(this).data('confirm') != 'undefined' && $(this).data('confirm') != '') {
                    if (window.confirm($(this).data('confirm'))) {
                        blnRespuesta = true;
                    }
                    else {
                        blnRespuesta = false;
                    }
                }

                //Si la variable es true, realizar la acción
                if (blnRespuesta) {

                    //Obtiene el formulario
                    var formulario = $(this);

                    //Serializa la forma para poder cargar la entidad
                    var datosCaptura = formulario.serialize();

                    //Obtiene el atributo donde se encuentra la URL
                    var strUrl = $(this).attr('action');

                    //Inicializa la barra de progreso
                    NProgress.start();

                    // Hace la llamada post y envia los datos del formulario
                    axios.post(strUrl, datosCaptura)
                        .then(function (respuesta) {

                            //Valida el status regresado por el servidor
                            if (respuesta.status == 200) {

                                //Asigna la informacion contenida en la respuesta del servidor
                                var data = respuesta.data.Data || respuesta.data;

                                //Valida la respuesta del servidor
                                switch (data.estatus) {
                                    case 'Exito':
                                        //Despliega el mensaje que indica que todo esta correcto
                                        toastr.success(data.mensaje);

                                        //Valida si se debe recargar el grid
                                        if (bolRecargaGrid)
                                            $('#' + strNombreGrid).bootstrapTable('refresh');

                                        //Valida si contiene la clase que muestra la barra lateral derecha,
                                        //en caso de estar visible entonces la oculta
                                        if ($('.' + Default.classDivSideBar).hasClass(Default.classOpenSideBar))
                                            $('.' + Default.classDivSideBar).toggleClass(Default.classOpenSideBar);


                                        //Elimina el formulario que existe en el contenedor
                                        $('#' + Default.classDivContainerSideBar).html('');
                                        break;

                                    case 'Advertencia':
                                        //Despliega el mensaje de revision
                                        toastr.warning(data.mensaje);
                                        break;

                                    case 'Error':
                                        //Despliega el mensaje de error
                                        toastr.error(data.mensaje);
                                        break;

                                    default:
                                        //Si no viene catalogada la respuesta.
                                        //entonces envia un mensaje de informacion
                                        toastr.info(data.mensaje);
                                        break;
                                }//fin switch

                            }//fin if

                        })//fin then axios
                        .catch(function (errormessage) {
                            console.log(errormessage);
                            //Si existe un error entonces despliega los datos en una notificacion
                            if (errormessage.isAxiosError)
                                toastr.error('Existe un error, contacte al administrador: ' + errormessage.message);
                            else
                                toastr.error('Existe un error, contacte al administrador');

                        })//fin catch axios
                        .then(function () {
                            NProgress.done();
                        }); //fin cualquier otra cosa axios

                }//if blRespuesta
                
            }//if validacion cliente

        });

        return JsBase;

    }//EnviarFormulario

    /**
     * Agrega el evento para ocultar la barra lateral derecha, utiliza las opciones por defecto
     * @param {string} [strClaseOcultarBarra] - Nombre de la clase que identifica el objeto que desencadena el evento para ocultar la barra
     *
     * @Uso:     JsBase.ocultarBarraLateral();
     * @Nota:    Si se requiere modificar las opciones por defecto, entonces se deben modificar las opciones con el metodo configurar
     * 
     * @Autor:   ngr <2020/05/15>
     * @Modif:   xxx <>
     */
    JsBase.ocultarBarraLateral = (strClaseOcultarBarra) => {

        $('.' + strClaseOcultarBarra).click(function () {
            //Oculta la barra lateral derecha
            $('.' + Default.classDivSideBar).toggleClass(Default.classOpenSideBar);
            //Elimina el formulario que existe en el contenedor
            $('#' + Default.classDivContainerSideBar).html('');
        });        

        return JsBase;

    }//ocultarBarraLateral

    /**
     * Realiza una acción y solicita la confirmación de la misma del id seleccionado (por ejemplo eliminar)
     * @param {string} [strClase] - Nombre de la clase que identifica el objeto que desencadena el evento que genera una accion con confirmacion
     * @param {boolean} [bolMensajeEliminar] - Indica si debe mostrar el mensaje de eliminar o uno generico
     * @param {boolean} [bolRecargaGrid] - Indica si debe recargar el grid
     * @param {string} [strNombreGrid] - Nombre de grid que se va recargar
     *
     * @Uso:    jsBase.confirmarRegistro('Eliminar/5');
     * @Nota:   Puede moficiar la configuracion por defecto para modificar el mensaje de eliminar
     *          Se requiere el nombre de la clase y el atributo en el div que contiene el link o en el boton de accion
     *              agregar la clase class="eliminar-registro-confirmacion"
     *              agregar el tributo data-url="/Controlador/Accion/Id" Donde el valor es la ruta del controlador con su accion y su identificador
     * @Autor:  rnava@cdi.gob.mx <2017/06/20>
     * @Modif:  xxxx@cdi.gob.mx <>
    */
    JsBase.confirmarRegistro = function (strClase, bolMensajeEliminar, bolRecargaGrid, strNombreGrid) {

        //Agrega la funcionalidad al evento clic
        $('.' + strClase).click(function () {

            //Obtien la url de los incluida en el control
            var strUrl = $(this).data('url');

            JsBase.confirmarRegistroURL(strUrl, bolMensajeEliminar, bolRecargaGrid, strNombreGrid);

        });

        return JsBase;

    }//confirmarRegistro

    /**
     * Realiza una acción y solicita la confirmación de la misma del id seleccionado (por ejemplo eliminar)
     * @param {string} [strURL] URL de la accion en el controlador
     * @param {boolean} [bolMensajeEliminar] - Indica si debe recargar el grid
     * @param {boolean} [bolRecargaGrid] - Indica si debe recargar el grid
     * @param {string} [strNombreGrid] - Nombre de grid que se va recargar
     *
     * @Uso:    jsBase.confirmarRegistro('Eliminar/5');
     * @Nota:   Puede moficiar la configuracion por defecto para modificar el mensaje de eliminar
     * @Nota:   Se requiere el nombre de la clase y el atributo en el div que contiene el link eliminar o en el boton de eliminar
     *              agregar la clase class="eliminar-registro" 
     *              agregar el tributo data-url="/Controlador/Accion/Id" Donde el valor es la ruta del controlador con su accion y su identificador
     * @Autor:  rnava@cdi.gob.mx <2017/06/20>
     * @Modif:  xxxx@cdi.gob.mx <>
    */
    JsBase.confirmarRegistroURL = function (strUrl, bolMensajeEliminar, bolRecargaGrid, strNombreGrid) {

        //Muestra el mensaje de confirmacion y valida la opcion seleccionada
        if (confirm(bolMensajeEliminar ? Default.mensajeEliminar : Default.mensajeConfirmacion)) {

            //Inicializa la barra de progreso
            NProgress.start();

            // Hace la llamada post y envia los datos del formulario
            axios.get(strUrl)
                .then(function (respuesta) {

                    //Valida el status regresado por el servidor
                    if (respuesta.status == 200) {

                        //////Valida si se debe redireccionar porque la sesion termino
                        ////if (JSON.parse(respuesta.headers['x-responded-json']).status == 401) {
                        ////    window.location.href = JSON.parse(respuesta.headers['x-responded-json']).headers.location;
                        ////}
                        ////else {

                            //Asigna la informacion contenida en la respuesta del servidor
                            var data = respuesta.data.Data || respuesta.data;

                            //Valida la respuesta del servidor
                            switch (data.estatus) {
                                case 'Exito':
                                    //Despliega el mensaje que indica que todo esta correcto
                                    toastr.success(data.mensaje);

                                    //Valida si se debe recargar el grid
                                    if (bolRecargaGrid)
                                        $('#' + strNombreGrid).bootstrapTable('refresh');

                                    break;

                                case 'Advertencia':
                                    //Despliega el mensaje de revision
                                    toastr.warning(data.mensaje);
                                    break;

                                case 'Error':
                                    //Despliega el mensaje de error
                                    toastr.error(data.mensaje);
                                    break;

                                default:
                                    //Si no viene catalogada la respuesta.
                                    //entonces envia un mensaje de informacion
                                    toastr.info(data.mensaje);
                                    break;
                            }//fin switch

                        //}if                        

                    }//fin if
                    else {
                        toastr.error('Existe un error, contacte al administrador. ErrorCode' + respuesta.status);
                    }

                })//fin then axios
                .catch(function (errormessage) {
                    console.log(errormessage);
                    //Si existe un error entonces despliega los datos en una notificacion
                    if (errormessage.isAxiosError)
                        toastr.error('Existe un error, contacte al administrador: ' + errormessage.message);
                    else
                        toastr.error('Existe un error, contacte al administrador');

                })//fin catch axios
                .then(function () {
                    NProgress.done();
                }); //fin cualquier otra cosa axios

        }//if confirmacion

        return JsBase;

    }//confirmarRegistroURL

    return JsBase;

})));
