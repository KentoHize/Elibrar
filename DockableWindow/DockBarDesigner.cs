using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Aritiafel.Organizations.ElibrarPartFactory
{
    public class DockBarDesigner : ControlDesigner
    {
        protected DockBar control;
        public DockBarDesigner()
        { }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            control = component as DockBar;
        }

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            control.Dock = DockBar.DefaultDockStyle;
        }
    }
}
