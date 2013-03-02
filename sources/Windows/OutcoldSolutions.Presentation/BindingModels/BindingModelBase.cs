// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The binding model base.
    /// </summary>
    public class BindingModelBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, List<EventHandler<PropertyChangedEventArgs>>> subscriptions =
            new Dictionary<string, List<EventHandler<PropertyChangedEventArgs>>>();

        private List<string> notifications;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Subscribe for property changed notification for special field.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="expression"/> or <paramref name="action"/> is null.
        /// </exception>
        public void Subscribe(
            Expression<Func<object>> expression,
            EventHandler<PropertyChangedEventArgs> action)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.Subscribe(PropertyNameExtractor.GetPropertyName(expression), action);
        }

        /// <summary>
        /// Unsubscribe for property changed notification for special field.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="expression"/> or <paramref name="action"/> is null.
        /// </exception>
        public void Unsubscribe(
            Expression<Func<object>> expression,
            EventHandler<PropertyChangedEventArgs> action)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.Unsubscribe(PropertyNameExtractor.GetPropertyName(expression), action);
        }

        /// <summary>
        /// Subscribe for property changed notification for special field.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="propertyName"/> or <paramref name="action"/> is null.
        /// </exception>
        public void Subscribe(
            string propertyName,
            EventHandler<PropertyChangedEventArgs> action)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            lock (this.subscriptions)
            {
                List<EventHandler<PropertyChangedEventArgs>> propertySubscriptions;
                if (this.subscriptions.TryGetValue(propertyName, out propertySubscriptions))
                {
                    propertySubscriptions.Add(action);
                }
                else
                {
                    this.subscriptions.Add(propertyName, new List<EventHandler<PropertyChangedEventArgs>> { action });
                }
            }
        }

        /// <summary>
        /// Unsubscribe for property changed notification for special field.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="propertyName"/> or <paramref name="action"/> is null.
        /// </exception>
        public void Unsubscribe(
            string propertyName,
            EventHandler<PropertyChangedEventArgs> action)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            lock (this.subscriptions)
            {
                List<EventHandler<PropertyChangedEventArgs>> propertySubscriptions;
                if (this.subscriptions.TryGetValue(propertyName, out propertySubscriptions))
                {
                    propertySubscriptions.Remove(action);
                }
            }
        }

        /// <summary>
        /// Clear all property changed subscriptions.
        /// </summary>
        public void ClearPropertyChangedSubscriptions()
        {
            this.PropertyChanged = null;

            lock (this.subscriptions)
            {
                this.subscriptions.Clear();
            }
        }

        /// <summary>
        /// Freeze property change notifications.
        /// </summary>
        public void FreezeNotifications()
        {
            this.notifications = new List<string>();
        }

        /// <summary>
        /// Unfreeze property change notifications.
        /// </summary>
        public void UnfreezeNotifications()
        {
            var propertyChangeNotifications = this.notifications;
            this.notifications = null;

            if (propertyChangeNotifications != null)
            {
                foreach (var notification in propertyChangeNotifications)
                {
                    this.RaisePropertyChanged(notification);
                }
            }
        }

        /// <summary>
        /// Set the value to the field.
        /// </summary>
        /// <typeparam name="T">
        /// The field type.
        /// </typeparam>
        /// <param name="fieldValue">
        /// The field value.
        /// </param>
        /// <param name="value">
        /// The new value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>. If <paramref name="fieldValue"/> is the same as <paramref name="value"/> - false will be return.
        /// </returns>
        protected bool SetValue<T>(ref T fieldValue, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(fieldValue, value))
            {
                return false;
            }

            fieldValue = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raise property changed.
        /// </summary>
        /// <param name="expression">
        /// The expression, which contains property.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="expression"/> is null.
        /// </exception>
        protected void RaisePropertyChanged(Expression<Func<object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.RaisePropertyChanged(PropertyNameExtractor.GetPropertyName(expression));
        }

        /// <summary>
        /// Raise property changed for current method.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void RaiseCurrentPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.RaisePropertyChanged(propertyName);
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.notifications != null)
            {
                if (!this.notifications.Contains(propertyName))
                {
                    this.notifications.Add(propertyName);
                }

                return;
            }

            var eventArgs = new PropertyChangedEventArgs(propertyName);

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

            List<EventHandler<PropertyChangedEventArgs>> propertySubscriptions = null;

            lock (this.subscriptions)
            {
                List<EventHandler<PropertyChangedEventArgs>> result;
                if (this.subscriptions.TryGetValue(propertyName, out result))
                {
                    propertySubscriptions = result.ToList();
                }
            }

            if (propertySubscriptions != null)
            {
                foreach (var propertySubscription in propertySubscriptions)
                {
                    propertySubscription(this, eventArgs);
                }
            }
        }
    }
}
