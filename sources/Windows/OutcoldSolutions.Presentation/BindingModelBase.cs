// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
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
