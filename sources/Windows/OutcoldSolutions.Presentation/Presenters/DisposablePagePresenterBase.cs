// --------------------------------------------------------------------------------------------------------------------
// OutcoldSolutions (http://outcoldsolutions.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presenters
{
    using System;
    using System.Collections.Generic;

    using OutcoldSolutions.BindingModels;
    using OutcoldSolutions.Views;

    /// <summary>
    /// The disposable page presenter base.
    /// </summary>
    /// <typeparam name="TView">
    /// The view type.
    /// </typeparam>
    /// <typeparam name="TBindingModel">
    /// The type of binding model.
    /// </typeparam>
    public abstract class DisposablePagePresenterBase<TView, TBindingModel> : PagePresenterBase<TView, TBindingModel>
        where TView : IPageView 
        where TBindingModel : BindingModelBase
    {
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        ~DisposablePagePresenterBase()
        {
            this.Dispose(disposing: false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Register object for dispose. It will be disposed when <see cref="Dispose"/> will be called.
        /// </summary>
        /// <param name="disposable">
        /// The disposable object.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="disposable"/> is null.
        /// </exception>
        protected void RegisterForDispose(IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException("disposable");
            }

            this.disposables.Add(disposable);
        }

        /// <summary>
        /// Method will be called when <see cref="Dispose"/> will be invoked.
        /// </summary>
        protected virtual void OnDisposing()
        {
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.OnDisposing();
                this.BindingModel.DisposeIfDisposable();

                foreach (var disposable in this.disposables)
                {
                    disposable.Dispose();
                }

                this.disposables.Clear();
            }
        }
    }
}