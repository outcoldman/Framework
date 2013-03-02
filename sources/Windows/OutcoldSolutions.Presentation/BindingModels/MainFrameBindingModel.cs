// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.BindingModels
{
    internal class MainFrameBindingModel : BindingModelBase
    {
        private bool isBackButtonVisible;

        public bool IsBackButtonVisible
        {
            get
            {
                return this.isBackButtonVisible;
            }

            set
            {
                this.SetValue(ref this.isBackButtonVisible, value);
            }
        }
    }
}