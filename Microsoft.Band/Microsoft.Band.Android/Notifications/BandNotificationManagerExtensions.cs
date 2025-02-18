using System;
using System.Threading.Tasks;

namespace Microsoft.Band.Notifications
{
    public static class BandNotificationManagerExtensions
    {
        public static Task SendMessageTaskAsync(this IBandNotificationManager manager, Java.Util.UUID tileId, string title, string body, Java.Util.Date date, MessageFlags flags)
        {
            return manager.SendMessageAsync(tileId, title, body, date, flags).AsTask();
        }

        public static Task SendMessageTaskAsync(this IBandNotificationManager manager, Java.Util.UUID tileId, string title, string body, DateTime date, MessageFlags flags)
        {
			var javaDate = new Java.Util.Date(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
			return manager.SendMessageTaskAsync(tileId, title, body, javaDate, flags);
        }

        public static Task SendMessageTaskAsync(this IBandNotificationManager manager, Java.Util.UUID tileId, string title, string body, DateTime date, bool showDialog)
        {
            return manager.SendMessageTaskAsync(tileId, title, body, date, showDialog ? MessageFlags.ShowDialog : MessageFlags.None);
        }

        public static Task SendMessageTaskAsync(this IBandNotificationManager manager, Java.Util.UUID tileId, string title, string body, DateTime date)
        {
            return manager.SendMessageTaskAsync(tileId, title, body, date, false);
        }

        public static Task ShowDialogTaskAsync(this IBandNotificationManager manager, Java.Util.UUID tileId, string title, string body)
        {
            return manager.ShowDialogAsync(tileId, title, body).AsTask();
        }

        public static Task VibrateTaskAsync(this IBandNotificationManager manager, VibrationType type)
        {
            return manager.VibrateAsync(type).AsTask();
        }
    }
}