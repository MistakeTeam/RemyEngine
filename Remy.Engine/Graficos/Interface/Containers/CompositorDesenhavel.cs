namespace Remy.Engine.Graficos.Interface.Containers
{
    public class CompositorDesenhavel : Desenhavel
    {
        public CompositorDesenhavel()
        {
            internalChildren = new List<Desenhavel>();
        }

        protected List<Desenhavel> internalChildren;

        protected virtual void AddInternal(Desenhavel desenhavel)
        {
            desenhavel.Parent = this;
            internalChildren.Add(desenhavel);
        }

        public override bool UpdateSubTree()
        {
            if (!base.UpdateSubTree()) return false;

            for (int i = 0; i < internalChildren.Count; ++i)
            {
                internalChildren[i].UpdateSubTree();
            }

            return true;
        }

        public override bool UpdateDrawSubTree()
        {
            if (!base.UpdateDrawSubTree()) return false;

            for (int i = 0; i < internalChildren.Count; ++i)
            {
                internalChildren[i].UpdateDrawSubTree();
                internalChildren[i].Dispose();
            }

            return true;
        }
    }
}