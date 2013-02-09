// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation.Suites
{
    using System.ComponentModel;

    using Moq;

    using NUnit.Framework;

    using OutcoldSolutions.Presentation;

    public class BindingModelBaseSuites : SuitesBase
    {
        private Mock<ISubscriber> subscriber;
        private BindingModel bindingModel;

        public override void SetUp()
        {
            base.SetUp();

            this.subscriber = new Mock<ISubscriber>();
            this.bindingModel = new BindingModel();
        }

        [Test]
        public void Subscribe_SubscribeOnPropertyChange_MethodNotified()
        {
            // Arrange
            this.bindingModel.Subscribe(() => this.bindingModel.Property1, this.subscriber.Object.SubscribeMethod);

            // Act
            this.bindingModel.Property1 = 10;

            // Assert
            this.subscriber.Verify(
                x =>
                x.SubscribeMethod(
                    this.bindingModel,
                    It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertyNameExtractor.GetPropertyName(() => this.bindingModel.Property1))),
                Times.Once());
        }

        [Test]
        public void Unsubscribe_UnsubscribeOnPropertyChange_MethodIsNotNotified()
        {
            // Arrange
            this.bindingModel.Subscribe(() => this.bindingModel.Property1, this.subscriber.Object.SubscribeMethod);
            this.bindingModel.Unsubscribe(() => this.bindingModel.Property1, this.subscriber.Object.SubscribeMethod);

            // Act
            this.bindingModel.Property1 = 10;

            // Assert
            this.subscriber.Verify(
                x =>
                x.SubscribeMethod(
                    this.bindingModel,
                    It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertyNameExtractor.GetPropertyName(() => this.bindingModel.Property1))),
                Times.Never());
        }

        [Test]
        public void PropertyChanged_SubscribeOnPropertyChange_MethodNotified()
        {
            // Arrange
            this.bindingModel.PropertyChanged += this.subscriber.Object.SubscribeMethod;

            // Act
            this.bindingModel.Property1 = 10;

            // Assert
            this.subscriber.Verify(
                x =>
                x.SubscribeMethod(
                    this.bindingModel,
                    It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertyNameExtractor.GetPropertyName(() => this.bindingModel.Property1))),
                Times.Once());
        }

        [Test]
        public void PropertyChanged_UnsubscribeOnPropertyChange_MethodIsNotNotified()
        {
            // Arrange
            this.bindingModel.PropertyChanged += this.subscriber.Object.SubscribeMethod;
            this.bindingModel.PropertyChanged -= this.subscriber.Object.SubscribeMethod;

            // Act
            this.bindingModel.Property1 = 10;

            // Assert
            this.subscriber.Verify(
                x =>
                x.SubscribeMethod(
                    this.bindingModel,
                    It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertyNameExtractor.GetPropertyName(() => this.bindingModel.Property1))),
                Times.Never());
        }

        [Test]
        public void ClearPropertyChangedSubscriptions_SubscriptionByName_MethodIsNotNotified()
        {
            // Arrange
            this.bindingModel.Subscribe(() => this.bindingModel.Property1, this.subscriber.Object.SubscribeMethod);
            this.bindingModel.ClearPropertyChangedSubscriptions();

            // Act
            this.bindingModel.Property1 = 10;

            // Assert
            this.subscriber.Verify(
                x =>
                x.SubscribeMethod(
                    this.bindingModel,
                    It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertyNameExtractor.GetPropertyName(() => this.bindingModel.Property1))),
                Times.Never());
        }

        [Test]
        public void ClearPropertyChangedSubscriptions_SubscriptionByEvent_MethodIsNotNotified()
        {
            // Arrange
            this.bindingModel.PropertyChanged += this.subscriber.Object.SubscribeMethod;
            this.bindingModel.ClearPropertyChangedSubscriptions();

            // Act
            this.bindingModel.Property1 = 10;

            // Assert
            this.subscriber.Verify(
                x =>
                x.SubscribeMethod(
                    this.bindingModel,
                    It.Is<PropertyChangedEventArgs>(e => e.PropertyName == PropertyNameExtractor.GetPropertyName(() => this.bindingModel.Property1))),
                Times.Never());
        }

        public interface ISubscriber
        {
            void SubscribeMethod(object sender, PropertyChangedEventArgs eventArgs);
        }

        public class BindingModel : BindingModelBase
        {
            private int property1Field = 0;

            public int Property1
            {
                get
                {
                    return this.property1Field;
                }

                set
                {
                    this.property1Field = value;
                    this.RaiseCurrentPropertyChanged();
                }
            }
        }
    }
}