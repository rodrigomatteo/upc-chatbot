using System.ComponentModel.DataAnnotations;

namespace Upecito.Model.Common
{
    public class Enums
    {

        public enum ThemeApp
        {
            Dark,
            White
        }

        public enum WhatsAppProvider
        {
            ApiWha,
            NiceApi
        }

        public enum EmailProvider
        {
            Gmail,
            Outlook
        }

        public enum SmsProvider
        {
            InfoBip,
            Twilio
        }

        public enum TypeRole
        {
            View, Method, Other
        }

        public enum ScheduleStatus
        {
            NOTSCHEDULED,
            SCHEDULED,
            IN_PROGRESS,
            FINISHED,
            CANCELED
        }

        //https://developers.facebook.com/docs/messenger-platform/send-messages#messaging_types
        public enum MessagingType
        {
            MESSAGE_TAG,
            RESPONSE,
            UPDATE
        }

        public enum StatusConversation
        {
            Abierta,
            Derivada,
            Cerrada
        }

        public enum ConversationSource
        {
            Facebook,
            WhatsApp,
            Skype,
            Slack,
            Google,
            Web,
            Email,
            SMS,
            Other
        }

        public enum SettingType
        {
            [Display(Name = "Cadena de Texto")]
            String,
            [Display(Name = "Entero")]
            Integer,
            [Display(Name = "Decimal")]
            Decimal,
            [Display(Name = "Boleano")]
            Boolean,
            [Display(Name = "FechaHora")]
            DateTime,
            [Display(Name = "Largo")]
            Long,
            [Display(Name = "Lista de Textos")]
            StringArray,
        }

        public enum StatusPerson
        {
            [Display(Name = "Activo")]
            Activo,
            [Display(Name = "Inactivo")]
            Inactivo
        }

        public enum SubscriptionType
        {
            [Display(Name = "Suscrito")]
            Suscribed,
            [Display(Name = "De baja")]
            Unsuscribed,
            [Display(Name = "No configurado")]
            NoSet
        }

        public enum NotificationType
        {
            [Display(Name = "No Notificacdo")]
            UnNotified,
            [Display(Name = "Notificado Regla Tiempo")]
            NotifiedOnTimer
        }


        public enum StatusState
        {
            [Display(Name = "OK")]
            Success,
            Error
        }

        public enum HttpMethod
        {
            GET,
            POST,
            UPDATE,
            DELETE
        }

        public enum TriggerStatus
        {
            Success, Error, Exception
        }

        public enum TypeRequestAPI
        {
            Get, Post, Put, Delete, CallBack
        }

        public enum PhoneType
        {
            Celular, Fijo
        }

        public enum Operator
        {
            Entel, Claro, Movistar, Bitel, Otro
        }


        #region Facebook

        public enum Gender
        {
            [Display(Name = "Masculino")]
            Male,
            [Display(Name = "Femenino")]
            Female,
            [Display(Name = "Otro")]
            Other
        }

        public enum ConversationType
        {
            Consulta, Respuesta
        }

        public enum ResponseSource
        {
            Watson,
            Offline,
            Agent,
            User
        }

        public enum TypeButton
        {
            postback,
            web_url,
            phone_number,
            element_share
        }

        public enum QrType
        {
            [Display(Name = "Texto")]
            text,
            [Display(Name = "Ubicación")]
            location,
            [Display(Name = "Teléfono")]
            user_phone_number,
            [Display(Name = "Correo Electrónico")]
            user_email
        }

        public enum OperatorPredicate
        {
            OR,
            NOT,
            AND
        }

        public enum NotificationTypeBroadcast
        {
            REGULAR, //sound/vibration (default)
            SILENT_PUSH, //on-screen notification only
            NO_PUSH //no notification
        }

        public enum FbObjectType
        {
            Text,
            Image,
            QuickReply,
            Carousel,
            ButtonTemplate
        }

        public enum EnumsBroadcast
        {
            Update,
            Suscription,
            Shipping,
            Reservation,
            Issue,
            Appointment,
            Game,
            Transportation,
            Feature,
            Ticket,
            Account,
            Payment,
            PersonalFinance,
            Pairing,
            Application,
            EventReminder,
            Alert
        }

        public enum AttachmentType
        {
            text,
            audio,
            video,
            image,
            file
        }


        public enum GroupBlockType
        {
            Block,
            Sequence
        }

        public enum ResponseType
        {
            success,
            error,
            warning,
            info
        }

        public enum SurveyStep
        {
            Satisfaction,
            Resolution,
            Finished
        }
        #endregion

    }
}
