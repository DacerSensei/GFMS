using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace GFMSLibrary
{
    

    public class ObservableHashSet<T> : ObservableCollection<T>
    {
        public ObservableHashSet()
        {
        }

        public ObservableHashSet(IEnumerable<T> collection) : base(collection)
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        protected override void InsertItem(int index, T item)
        {
            if (!Contains(item))
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (!Contains(item))
            {
                base.SetItem(index, item);
            }
        }

        public new void Add(T item)
        {
            if (!Contains(item))
            {
                base.Add(item);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }
    }
}
