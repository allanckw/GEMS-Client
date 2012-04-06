﻿using System.Windows.Controls;
using System.Windows;

namespace Gems.UIWPF
{
    public class GEMSPage : Page
    {
        protected bool changed = false;
        public bool isChanged() { return changed; }
        public virtual bool saveChanges() { return true; }
        protected void onChanged(object sender, RoutedEventArgs e) { changed = true; }
    }
}
