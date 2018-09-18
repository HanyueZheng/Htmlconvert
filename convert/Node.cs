using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace convert
{
    public class Node
    {
        private Node Parent;
        private List<Node> Childs = new List<Node>();
        private string nodename;
        private string text;

        public Node()
        {
           
        }

        public Node(Node parent, List<Node> Childs, string nodename, string text)
        {
            this.Parent = parent;
            this.Childs = Childs;
            this.nodename = nodename;
            this.text = text;
        }

        public void AddChild(Node n)
        {
            Childs.Add(n);
        }

        public void setNodename(string nodename)
        {
            this.nodename = nodename;
        }

        public void setParent(Node Parent)
        {
            this.Parent = Parent;
        }

        public void setText(string text)
        {
            this.text = text;
        }

        public List<Node> getChilds()
        {
            return this.Childs;
        }

        public string getNodename()
        {
            return this.nodename;
        }

        public string getText()
        {
            return this.text;
        }

        public Node getParent()
        {
            return this.Parent;
        }
    }
}
