using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace Data.Layer
{
    interface ISavableObject
    {
        void BeforeSave(ObjectStateEntry stateEntry);
    }

    public class ObjectContextFactory
    {
        private const EntityState ChangedEntityStates = EntityState.Added | EntityState.Modified | EntityState.Deleted;

        static void ContextSavingChanges(object sender, EventArgs e)
        {
            ObjectContext context = sender as ObjectContext;
            IEnumerable<ObjectStateEntry> stateEntries = context.ObjectStateManager
            .GetObjectStateEntries(ChangedEntityStates)
            .Where(stateEntry => !stateEntry.IsRelationship);
            foreach (ObjectStateEntry stateEntry in stateEntries)
                BeforeSaveChanges(stateEntry);
        }

        public static ObjectContext GetContext()
        {
            System.Data.Objects.ObjectContext context = ((ObjectContext)new VEDKEntities());
            context.SavingChanges += ContextSavingChanges;
            return context;
        }

        private static void BeforeSaveChanges(ObjectStateEntry stateEntry)
        {
            var savable = stateEntry.Entity as ISavableObject;
            if (savable != null)
                savable.BeforeSave(stateEntry);
        }

    }
}
