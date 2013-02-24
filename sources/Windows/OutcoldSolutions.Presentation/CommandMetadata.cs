// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The command metadata.
    /// </summary>
    public class CommandMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMetadata"/> class.
        /// </summary>
        /// <param name="iconName">
        /// The icon name.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="iconName"/> or <paramref name="command"/> are null.
        /// </exception>
        public CommandMetadata(string iconName, DelegateCommand command)
        {
            if (iconName == null)
            {
                throw new ArgumentNullException("iconName");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            this.IconName = iconName;
            this.Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMetadata"/> class.
        /// </summary>
        /// <param name="iconName">
        /// The icon name.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="iconName"/> or <paramref name="command"/> or <paramref name="title"/> are null.
        /// </exception>
        public CommandMetadata(string iconName, string title, DelegateCommand command)
            : this(iconName, command)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            this.Title = title;
        }

        /// <summary>
        /// Gets or sets the icon name.
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public DelegateCommand Command { get; set; }
    }
}