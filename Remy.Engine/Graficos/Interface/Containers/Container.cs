using System.Collections;
using Remy.Engine.Logs;

namespace Remy.Engine.Graficos.Interface.Containers
{
    public class Container : Container<Desenhavel>
    {
        public Container(string name = "") : base(name) { }
    }

    public class Container<T> : CompositorDesenhavel, ICollection<T>, IReadOnlyList<T> where T : Desenhavel
    {
        public Container(string name = "")
        {
            Logger.WriteLine("Novo container " + name);
        }

        protected virtual Container<T> Content => this;

        public IReadOnlyList<T> Children
        {
            get
            {
                if (Content != this)
                    return Content.Children;

                return (IReadOnlyList<T>)internalChildren;
            }
        }

        public int Count => Children.Count;

        public bool IsReadOnly => false;

        public T this[int index] => Children[index];

        public T Child
        {
            get
            {
                if (Children.Count != 1)
                    throw new InvalidOperationException($"Cannot call {nameof(Child)} unless there's exactly one {nameof(Desenhavel)} in {nameof(Children)} (currently {Children.Count})!");

                return Children[0];
            }
            set
            {
                if (IsDisposed)
                    return;

                // Clear();
                Add(value);
            }
        }

        public void Add(T desenhavel)
        {
            if (desenhavel == Content)
                throw new InvalidOperationException("Content may not be added to itself.");

            ArgumentNullException.ThrowIfNull(desenhavel);
            ObjectDisposedException.ThrowIf(desenhavel.IsDisposed, desenhavel);

            if (Content == this)
                base.AddInternal(desenhavel);
            else
                Content.Add(desenhavel);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T desenhavel)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}