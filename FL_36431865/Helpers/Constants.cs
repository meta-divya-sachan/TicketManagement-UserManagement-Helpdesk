namespace TicketManagement.Helpers
{
    public class Constants
    {
        public class Reporte
        {
            public class Lista
            {
                public const int pCategoria = 1;
                public const int pProceso = 2;
                public const int pLineaNegocio = 3;
                public const int pUnidadFuncional = 4;
                public const int pClasificacionCausa = 5;
            }
        }

        public class Modulos
        {
            public const int Parametro = 1;
            public const int EventoDeRiesgo = 3;
            public const int Auditoria = 5;
        }

        public class Accion
        {
            public const int SinAccion = 0;
            public const int Creacion = 1;
            public const int Edicion = 2;
        }

        public class TipoEvento
        {
            public const string ER = "ER";
            public const string EP = "EP";
        }

        public class Tables
        {
            public const int RequestTypes = 1; 
            public const int RequestSituations = 2;
        }
    }
}
