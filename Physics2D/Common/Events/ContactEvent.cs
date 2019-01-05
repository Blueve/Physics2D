namespace Physics2D.Common.Events
{
    using System;
    using System.Collections.Generic;
    using Physics2D.Collision;

    public delegate void ContactHandle(object sender, ContactEventArgs e);

    public sealed class ContactEventArgs : EventArgs
    {
        public IReadOnlyList<ParticleContact> ContactList { get; }

        public ContactEventArgs(IReadOnlyList<ParticleContact> contact)
        {
            this.ContactList = contact;
        }
    }
}
