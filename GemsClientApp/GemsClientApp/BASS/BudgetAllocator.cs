using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using evmsService.entities;

namespace Gems.UIWPF
{
    class State : IComparable<State>
    {
        public decimal priceTotal;
        public int satisfactionTotal;
        public Items item;
        public State prevState;

        public int CompareTo(State s)
        {
            return priceTotal.CompareTo(s.priceTotal);
        }
    }

    public class BudgetAllocator
    {
        public decimal minBudget;
        public decimal maxBudget;
        public List<List<Items>> importantItemsByType;
        public List<List<Items>> unimportantItemsByType;
        List<State> DPtreeLeaves;

        public BudgetAllocator(List<Items> items, List<ItemTypes> itemTypes, decimal maxBudget)
        {
            importantItemsByType = new List<List<Items>>();
            unimportantItemsByType = new List<List<Items>>();

            Dictionary<string, List<Items>> itemsByType = new Dictionary<string,List<Items>>();
            foreach(Items item in items)
            {
                if(!itemsByType.ContainsKey(item.typeString))
                    itemsByType[item.typeString] = new List<Items>();
                itemsByType[item.typeString].Add(item);
            }
            foreach(ItemTypes itemType in itemTypes)
            {
                if (itemsByType.ContainsKey(itemType.typeString))
                    if (itemType.IsImportantType)
                        importantItemsByType.Add(itemsByType[itemType.typeString].OrderBy(i => i.EstimatedPrice).ToList());
                    else
                        unimportantItemsByType.Add(itemsByType[itemType.typeString]);
            }
            minBudget = 0;
            foreach (List<Items> itemsList in importantItemsByType)
                minBudget += itemsList[itemsList.Count - 1].EstimatedPrice;
            if (minBudget > maxBudget)
                throw new ArgumentOutOfRangeException("maxBudget", "Too small to obtain all required items");
            this.maxBudget = maxBudget;
            InitializeDPtree();
        }

        private void addItemToStates(Items item, IEnumerable<State> inStates, ICollection<State> outStates)
        {
            foreach (State state in inStates)
            {
                if (state.priceTotal + item.EstimatedPrice > maxBudget)
                    return;
                outStates.Add(new State()
                {
                    priceTotal = state.priceTotal + item.EstimatedPrice,
                    satisfactionTotal = state.satisfactionTotal + item.Satisfaction,
                    prevState = state,
                    item = item
                });
            }
        }

        public void InitializeDPtree()
        {
            List<State> prevStates = new List<State>();
            prevStates.Add(new State());
            foreach (List<Items> itemsList in importantItemsByType)
            {
                List<State> currStates = new List<State>();
                foreach (Items item in itemsList)
                    addItemToStates(item, prevStates, currStates);
                currStates.Sort();
                State prevState = new State();
                List<State> prunedStates = new List<State>();
                foreach (State currState in currStates)
                {
                    if (currState.satisfactionTotal > prevState.satisfactionTotal)
                    {
                        prunedStates.Add(currState);
                        prevState = currState;
                    }
                }
                prevStates = prunedStates;
            }
            foreach (List<Items> itemsList in unimportantItemsByType)
            {
                List<State> currStates = new List<State>();
                foreach (Items item in itemsList)
                    addItemToStates(item, prevStates, currStates);
                currStates.Sort();
                prevStates.Add(new State() { priceTotal = decimal.MaxValue });
                currStates.Add(new State() { priceTotal = decimal.MaxValue });
                List<State> prunedStates = new List<State>();
                State prevState = new State() { satisfactionTotal = -1 };
                for (int i = 0, j = 0; i < prevStates.Count || j < currStates.Count; )
                {
                    State currState = prevStates[i].priceTotal <
                        currStates[j].priceTotal ? prevStates[i++] : currStates[j++];
                    if (currState.priceTotal == decimal.MaxValue)
                        break;
                    if (currState.satisfactionTotal >= prevState.satisfactionTotal)
                    {
                        //Bug here should be >= not ==
                        prunedStates.Add(currState);
                        prevState = currState;
                    }
                }
                prevStates = prunedStates;
            }
            DPtreeLeaves = prevStates;
        }

        public List<List<Items>> optimalItems(decimal budget, out decimal priceTotal, out int satisfactionTotal)
        {
            if (budget < minBudget)
                throw new ArgumentOutOfRangeException("The amount of budget entered is insufficient");
            else if (budget > maxBudget)
            {
                throw new ArgumentOutOfRangeException("Your entered budget is more than your maximum defined budget!");
            }

            List<List<Items>> result = new List<List<Items>>();
            int i = DPtreeLeaves.Count - 1;
            for (; i >= 0; i--)
                if (DPtreeLeaves[i].priceTotal <= budget)
                    break;
            priceTotal = DPtreeLeaves[i].priceTotal;
            satisfactionTotal = DPtreeLeaves[i].satisfactionTotal;
            do
            {
                State stateOptimal = DPtreeLeaves[i];
                List<Items> itemsOptimal = new List<Items>();
                while (stateOptimal.prevState != null)
                {
                    itemsOptimal.Add(stateOptimal.item);
                    stateOptimal = stateOptimal.prevState;
                }
                result.Add(itemsOptimal);
            } while (--i >= 0 && DPtreeLeaves[i].priceTotal == priceTotal &&
                    DPtreeLeaves[i].satisfactionTotal == satisfactionTotal);
            return result;
        }
    }
}