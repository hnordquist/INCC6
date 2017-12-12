/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, 
such modified software should be clearly marked, so as not to confuse it with the version available from LANL.

Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided 
that the following conditions are met:
•	Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
disclaimer. 
•	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
disclaimer in the documentation and/or other materials provided with the distribution. 
•	Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, 
nor the names of its contributors may be used to endorse or promote products derived from this software without specific 
prior written permission. 
THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace UI.Logging
{
    /// <summary>
    /// Implements the <c>AutoScroll</c> attached property which indicates whether
    /// a <see cref="ListBox"/> will automatically scroll newly added items into view.
    /// </summary>
    public class AutoScrollBehavior : DependencyObject, IDisposable
    {
        /// <summary>
        /// Indicates whether a <see cref="ListBox"/> will automatically scroll newly added items into view.
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached(
                "AutoScroll",
                typeof(bool),
                typeof(AutoScrollBehavior),
                new UIPropertyMetadata(false, OnAutoScrollPropertyChanged));

        /// <summary>
        /// Gets the value of the <see cref="AutoScrollProperty"/> attached property from the specified <see cref="ListBox"/>.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/> from which to get the property value.</param>
        /// <returns>The property value.</returns>
        public static bool GetAutoScroll(ListBox listBox)
        {
            return (bool) listBox.GetValue(AutoScrollProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="AutoScrollProperty"/> attached property on the specified <see cref="ListBox"/>.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/> on which to set the property value.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetAutoScroll(ListBox listBox, bool value)
        {
            listBox.SetValue(AutoScrollProperty, value);
        }

        /// <summary>
        /// This method is called when the value of the <see cref="AutoScrollProperty"/> attached property changes.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> on which the property value changed.</param>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs"/> containing data for the event.</param>
        private static void OnAutoScrollPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ListBox listBox = obj as ListBox;

            if (listBox != null) {
                AutoScrollBehavior behavior;
                m_listBoxes.TryGetValue(listBox, out behavior);

                if ((bool) e.NewValue) {
                    if (behavior == null) {
                        behavior = new AutoScrollBehavior(listBox);
                        m_listBoxes.Add(listBox, behavior);
                    }
                }
                else {
                    if (behavior != null) {
                        behavior.Dispose();
                        m_listBoxes.Remove(listBox);
                    }
                }
            }
        }

        /// <summary>
        /// The collection to monitor for changes.
        /// </summary>
        private static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(AutoScrollBehavior),
                new PropertyMetadata(OnItemsSourcePropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoScrollBehavior"/> class.
        /// </summary>
        /// <param name="listBox">The associated <see cref="ListBox"/>.</param>
        private AutoScrollBehavior(ListBox listBox)
        {
            m_listBox = listBox;
            m_listBox.Loaded += OnLoaded;

            Binding binding = new Binding("ItemsSource");
            binding.Source = listBox;
            BindingOperations.SetBinding(this, ItemsSourceProperty, binding);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AutoScrollBehavior"/>.
        /// </summary>
        public void Dispose()
        {
            m_listBox.Loaded -= OnLoaded;

            if (m_scrollViewer != null) {
                m_scrollViewer.ScrollChanged -= OnScrollChanged;
            }

            BindingOperations.ClearBinding(this, ItemsSourceProperty);
        }

        /// <summary>
        /// This method is called when the <see cref="ListBox"/> is loaded.
        /// </summary>
        /// <param name="sender">The object that sent the event.</param>
        /// <param name="e">A <see cref="RoutedEventArgs"/> containing data for the event.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            m_scrollViewer = FindVisualChild<ScrollViewer>(m_listBox);

            if (m_scrollViewer != null) {
                m_scrollViewer.ScrollChanged += OnScrollChanged;
            }
        }

        /// <summary>
        /// This method is called when the <see cref="ListBox"/> scroll information changes.
        /// </summary>
        /// <param name="sender">The object that sent the event.</param>
        /// <param name="e">A <see cref="ScrollChangedEventArgs"/> containing data for the event.</param>
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0) {
                if (e.VerticalOffset == Math.Max(0, e.ExtentHeight - e.ViewportHeight)) {
                    m_isAutoScrollEnabled = true;
                }
                else {
                    m_isAutoScrollEnabled = false;
                }
            }
        }

        /// <summary>
        /// This method is called when the value of the <see cref="ListBox.ItemsSource"/> property changes.
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> on which the property value changed.</param>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs"/> containing data for the event.</param>
        private static void OnItemsSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((AutoScrollBehavior) obj).OnItemsSourceChanged(e);
        }

        /// <summary>
        /// This method is called when the value of the <see cref="ListBox.ItemsSource"/> property changes.
        /// </summary>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs"/> containing data for the event.</param>
        private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            INotifyCollectionChanged oldValue = e.OldValue as INotifyCollectionChanged;
            INotifyCollectionChanged newValue = e.NewValue as INotifyCollectionChanged;

            if (oldValue != null) {
                oldValue.CollectionChanged -= OnCollectionChanged;
            }

            if (newValue != null) {
                newValue.CollectionChanged += OnCollectionChanged;
            }
        }

        /// <summary>
        /// This method is called when the collection changes.
        /// </summary>
        /// <param name="sender">The object that sent the event.</param>
        /// <param name="e">A <see cref="NotifyCollectionChangedEventArgs"/> containing data for the event.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_scrollViewer != null) {
                if (e.Action == NotifyCollectionChangedAction.Add) {
                    if (m_isAutoScrollEnabled) {
                        m_scrollViewer.ScrollToBottom();
                    }
                }
            }
        }

        /// <summary>
        /// Finds a visual child of the specified <see cref="DependencyObject"/> with the specified type.
        /// </summary>
        /// <typeparam name="T">The type of child to find.</typeparam>
        /// <param name="parent">The <see cref="DependencyObject"/> that is the parent of the child to find.</param>
        /// <returns>The child, if found; otherwise, <c>null</c>.</returns>
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T) {
                    return (T) child;
                }
                else {
                    T grandchild = FindVisualChild<T>(child);

                    if (grandchild != null) {
                        return grandchild;
                    }
                }
            }

            return null;
        }

        private bool m_isAutoScrollEnabled = true;
        private ListBox m_listBox;
        private static IDictionary<ListBox, AutoScrollBehavior> m_listBoxes = new Dictionary<ListBox, AutoScrollBehavior>();
        private ScrollViewer m_scrollViewer;
    }
}
