namespace PrecompiledLibraries
{
    /// <summary>
    /// This represents the entity for service locator.
    /// </summary>
    public class MyServiceLocator
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MyServiceLocator"/> class.
        /// </summary>
        public MyServiceLocator()
        {
            this.Dependency = new MyDependency() { SomeValue = "Hello World" };
        }

        /// <summary>
        /// Gets or sets the <see cref="IDependency"/> instance.
        /// </summary>
        public IDependency Dependency { get; set; }
    }
}