using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using evmsService.entities;

namespace Gems.UIWPF
{
    class BudgetItemState
    {
        Items item;

        public Items Item
        {
            get { return item; }
            set { item = value; }
        }
        bool isBought;

        public bool IsBought
        {
            get { return isBought; }
            set { isBought = value; }
        }

        public BudgetItemState(Items item, bool isBought)
        {
            this.isBought = isBought;
            this.item = item;
        }
    }
}
