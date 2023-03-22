using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Unity.MemoryProfiler.Editor.Database.Operation;
using System;
using Unity.MemoryProfiler.Editor.UI.MemoryMap;
using Unity.MemoryProfiler.Editor.Database.Operation.Filter;
using Unity.MemoryProfiler.Editor.UIContentData;

namespace Unity.MemoryProfiler.Editor.UI
{
    internal class MemoryMapDiffPane : ViewPane
    {
        public override string ViewName { get { return TextContent.MemoryMapViewDiff.text; } }
        static class Content
        {
            public static readonly  GUIContent[] TableModesList = new GUIContent[3]
            {
                new GUIContent("Regions list"),
                new GUIContent("Allocations list"),
                new GUIContent("Objects list")
            };

            public static readonly  GUIContent FilterLabel = new GUIContent("Display Filters");
            public static readonly  GUIContent RowSizeLabel = new GUIContent("Row Size");
            public static readonly  GUIContent ColorSchemeLabel = new GUIContent("Color scheme");
        }

        internal class ViewStateHistory : ViewStateChangedHistoryEvent
        {
            public readonly MemoryMapDiffPane.TableDisplayMode TableDisplay;
            public readonly MemoryMapDiff.ViewState State;
            public readonly DatabaseSpreadsheet.State SpreadsheetState;
            public readonly bool FirstSnapshotActive;

            public ViewStateHistory(MemoryMapDiff.ViewState state, DatabaseSpreadsheet.State spreadsheetState, MemoryMapDiffPane.TableDisplayMode tableDisplay, bool firstSnapshotActive)
            {
                State = state;
                TableDisplay = tableDisplay;
                SpreadsheetState = spreadsheetState;
                FirstSnapshotActive = firstSnapshotActive;
            }

            protected override bool IsEqual(HistoryEvent evt)
            {
                var other = evt as ViewStateHistory;
                return other != null &&
                    State.Equals(other.State) &&
                    SpreadsheetState.Equals(other.SpreadsheetState) &&
                    TableDisplay == other.TableDisplay &&
                    FirstSnapshotActive == other.FirstSnapshotActive;
            }
        }

        internal class History : ViewOpenHistoryEvent
        {
            public override ViewStateChangedHistoryEvent ViewStateChangeRestorePoint => m_ViewState;
            ViewStateHistory m_ViewState;

            public History(MemoryMapDiffPane pane)
            {
                Mode = pane.m_UIState.CurrentMode;

                m_ViewState = pane.GetViewStateFilteringChangesSinceLastSelectionOrViewClose() as ViewStateHistory;
            }

            public void Restore(MemoryMapDiffPane pane, bool reopen, ViewStateChangedHistoryEvent viewStateToRestore = null, SelectionEvent selectionEvent = null, bool selectionIsLatent = false)
            {
                ViewStateHistory viewState = m_ViewState;
                if (viewStateToRestore != null && viewStateToRestore is ViewStateHistory)
                    viewState = viewStateToRestore as ViewStateHistory;

                if (selectionEvent != null)
                {
                    var tableState = viewState.SpreadsheetState;
                    tableState.SelectedRow = selectionEvent.Selection.RowIndex;
                    tableState.SelectionIsLatent = selectionIsLatent;
                    viewState = new ViewStateHistory(viewState.State, tableState, viewState.TableDisplay, viewState.FirstSnapshotActive);
                }

                pane.m_CurrentTableView = viewState.TableDisplay;
                pane.m_MemoryMap.CurrentViewState = viewState.State;

                if (viewState.FirstSnapshotActive)
                    pane.m_ActiveMode = pane.m_UIState.FirstMode as UIState.SnapshotMode;
                else
                    pane.m_ActiveMode = pane.m_UIState.SecondMode as UIState.SnapshotMode;

                pane.OnSelectRegions(viewState.State.HighlightedAddrMin, viewState.State.HighlightedAddrMax);
                pane.m_Spreadsheet.CurrentState = viewState.SpreadsheetState;
                pane.m_EventListener.OnRepaint();
            }

            public override string ToString()
            {
                return Mode.GetSchema().GetDisplayName() + seperator + "Memory Map Diff";
            }

            protected override bool IsEqual(HistoryEvent evt)
            {
                var hEvt = evt as History;
                if (hEvt == null)
                    return false;

                //TODO: change this to value type comparison once we figure out which data needs to be compared
                return ReferenceEquals(this, hEvt);
            }
        }

        MemoryMap.MemoryMapDiff m_MemoryMap;
        SnapshotAge m_CurrentFirstSnapshotAge;

        UI.DatabaseSpreadsheet m_Spreadsheet;

        public override VisualElement[] VisualElements
        {
            get
            {
                if (m_VisualElements == null)
                {
                    m_VisualElements = new VisualElement[]
                    {
                        new IMGUIContainer(() => OnGUI(0))
                        {
                            name = "MemoryMap",
                            style =
                            {
                                flexGrow = 6,
                            }
                        },
                        new IMGUIContainer(() => OnGUI(1))
                        {
                            name = "MemoryMapSpreadsheet",
                            style =
                            {
                                flexGrow = 2,
                            }
                        },
                        new IMGUIContainer(() => OnGUI(2))
                        {
                            name = "MemoryMapCallstack",
                            style =
                            {
                                flexGrow = 1,
                            }
                        }
                    };
                    m_VisualElementsOnGUICalls = new Action<Rect>[]
                    {
                        OnGUI,
                        OnGUISpreadsheet,
                        OnGUICallstack,
                    };
                }
                return m_VisualElements;
            }
        }
        public enum TableDisplayMode
        {
            Regions,
            Allocations,
            Objects,
        }
        UIState.SnapshotMode m_ActiveMode;

        TableDisplayMode m_CurrentTableView = TableDisplayMode.Regions;

        TableDisplayMode CurrentTableView
        {
            get
            {
                return m_CurrentTableView;
            }

            set
            {
                m_CurrentTableView = value;
                UnityEditor.EditorPrefs.SetInt("Unity.MemoryProfiler.Editor.UI.MemoryMapPaneDiff.TableDisplayMode", (int)m_CurrentTableView);
            }
        }

        public override bool ViewStateFilteringChangedSinceLastSelectionOrViewClose => m_ViewStateFilteringChangedSinceLastSelectionOrViewClose;
        bool m_ViewStateFilteringChangedSinceLastSelectionOrViewClose = false;

        GUIContent[] m_DisplayElementsList = null;
        GUIContent[] m_ColorSchemeList = null;

        struct RowSize
        {
            public readonly ulong Size;
            public readonly GUIContent Content;

            public RowSize(ulong size)
            {
                Size = size;

                if (size < 1024)
                    Content = new GUIContent(size.ToString() + " Bytes");
                else if (size < 1024 * 1024)
                    Content = new GUIContent((size / 1024).ToString() + " KB");
                else
                    Content = new GUIContent((size / (1024 * 1024)).ToString() + " MB");
            }
        };

        RowSize[] m_BytesInRowList = null;
        VisualElement m_ToolbarExtension;
        IMGUIContainer m_ToolbarExtensionPane;
        UIState.BaseMode m_ToolbarExtensionMode;

        void OnModeChanged(UIState.BaseMode newMode, UIState.ViewMode newViewMode)
        {
            if (m_ToolbarExtensionMode != null)
            {
                m_ToolbarExtensionMode.ViewPaneChanged -= OnViewPaneChanged;
                m_ToolbarExtensionMode = null;
            }

            if (newMode != null)
            {
                newMode.ViewPaneChanged += OnViewPaneChanged;
                m_ToolbarExtensionMode = newMode;
            }

            OnViewPaneChanged(newMode.CurrentViewPane);
        }

        void OnViewPaneChanged(ViewPane newPane)
        {
            if (m_ToolbarExtension.IndexOf(m_ToolbarExtensionPane) != -1)
            {
                m_ToolbarExtension.Remove(m_ToolbarExtensionPane);
            }

            if (newPane == this)
            {
                m_ToolbarExtension.Add(m_ToolbarExtensionPane);
            }
        }

        public MemoryMapDiffPane(IUIStateHolder s, IViewPaneEventListener l, VisualElement toolbarExtension)
            : base(s, l)
        {
            CurrentTableView = (TableDisplayMode)UnityEditor.EditorPrefs.GetInt("Unity.MemoryProfiler.Editor.UI.MemoryMapPaneDiff.TableDisplayMode", (int)TableDisplayMode.Regions);

            m_ToolbarExtension = toolbarExtension;
            m_ToolbarExtensionPane = new IMGUIContainer(new Action(OnGUIToolbarExtension));

            s.UIState.CurrentMode.ViewPaneChanged += OnViewPaneChanged;
            s.UIState.ModeChanged += OnModeChanged;

            string[] displayElements = Enum.GetNames(typeof(MemoryMap.MemoryMapDiff.DisplayElements));
            m_DisplayElementsList = new GUIContent[displayElements.Length];
            for (int i = 0; i < displayElements.Length; ++i)
                m_DisplayElementsList[i] = new GUIContent(displayElements[i]);

            string[] colorSchemes = Enum.GetNames(typeof(MemoryMap.MemoryMapDiff.ColorScheme));
            m_ColorSchemeList = new GUIContent[colorSchemes.Length];
            for (int i = 0; i < colorSchemes.Length; ++i)
                m_ColorSchemeList[i] = new GUIContent(colorSchemes[i]);

            ulong maxSize = 256 * 1024 * 1024; // 256,128,  64,32,16,8,  4,2,1,512,  256,128,64,32
            m_BytesInRowList = new RowSize[14];
            for (int i = 0; i < m_BytesInRowList.Length; ++i)
                m_BytesInRowList[i] = new RowSize(maxSize >> i);

            UIState.SnapshotMode mode1 = m_UIState.FirstMode as UIState.SnapshotMode;
            UIState.SnapshotMode mode2 = m_UIState.SecondMode as UIState.SnapshotMode;
            m_ActiveMode = mode1;

            m_CurrentFirstSnapshotAge = m_UIState.FirstSnapshotAge;
            m_MemoryMap = new MemoryMap.MemoryMapDiff();
            m_MemoryMap.Setup(mode1.snapshot, mode2.snapshot);
            m_MemoryMap.RegionSelected += OnSelectRegions;
        }

        public override void OnClose()
        {
            m_MemoryMap.Dispose();
            m_MemoryMap = null;
            m_Spreadsheet = null;
            m_ActiveMode = null;

            if (m_ToolbarExtensionMode != null)
                m_ToolbarExtensionMode.ViewPaneChanged -= OnViewPaneChanged;
            m_ToolbarExtensionMode = null;
        }

        void OnSelectRegions(ulong minAddr, ulong maxAddr)
        {
            m_ViewStateFilteringChangedSinceLastSelectionOrViewClose = true;
            var lr = new Database.LinkRequestTable();
            lr.LinkToOpen = new Database.TableLink();

            if (CurrentTableView == TableDisplayMode.Objects)
            {
                lr.LinkToOpen.TableName = ObjectAllTable.TableName;

                lr.LinkToOpen.RowWhere = new List<Database.View.Where.Builder>();
                lr.LinkToOpen.RowWhere.Add(new Database.View.Where.Builder("Address", Database.Operation.Operator.GreaterEqual, new Expression.MetaExpression(minAddr.ToString(), false)));
                lr.LinkToOpen.RowWhere.Add(new Database.View.Where.Builder("Address", Database.Operation.Operator.Less, new Expression.MetaExpression(maxAddr.ToString(), false)));
            }
            else if (CurrentTableView == TableDisplayMode.Allocations)
            {
                lr.LinkToOpen.TableName = "AllNativeAllocations";
                lr.LinkToOpen.RowWhere = new List<Database.View.Where.Builder>();
                lr.LinkToOpen.RowWhere.Add(new Database.View.Where.Builder("address", Database.Operation.Operator.GreaterEqual, new Expression.MetaExpression(minAddr.ToString(), false)));
                lr.LinkToOpen.RowWhere.Add(new Database.View.Where.Builder("address", Database.Operation.Operator.Less, new Expression.MetaExpression(maxAddr.ToString(), false)));
            }
            else if (CurrentTableView == TableDisplayMode.Regions)
            {
                lr.LinkToOpen.TableName = "RawAllMemoryRegions";// "RawNativeMemoryRegions";
                lr.LinkToOpen.RowWhere = new List<Database.View.Where.Builder>();
                lr.LinkToOpen.RowWhere.Add(new Database.View.Where.Builder("addressBase", Database.Operation.Operator.GreaterEqual, new Expression.MetaExpression(minAddr.ToString(), false)));
                lr.LinkToOpen.RowWhere.Add(new Database.View.Where.Builder("addressBase", Database.Operation.Operator.Less, new Expression.MetaExpression(maxAddr.ToString(), false)));
            }

            lr.SourceTable = null;
            lr.SourceColumn = null;
            lr.SourceRow = -1;

            OpenLinkRequest(lr, true);

            if (m_Spreadsheet.RowCount > 0)
            {
                m_Spreadsheet.SelectedRow = 0;
            }
        }

        public override UI.ViewOpenHistoryEvent GetOpenHistoryEvent()
        {
            return new History(this);
        }

        public override ViewStateChangedHistoryEvent GetViewStateFilteringChangesSinceLastSelectionOrViewClose()
        {
            m_ViewStateFilteringChangedSinceLastSelectionOrViewClose = false;
            var viewState = new ViewStateHistory(m_MemoryMap.CurrentViewState, m_Spreadsheet?.CurrentState ?? new DatabaseSpreadsheet.State(), m_CurrentTableView, m_ActiveMode == m_UIState.FirstMode);
            viewState.ChangeType = ViewStateChangedHistoryEvent.StateChangeType.FiltersChanged;
            return viewState;
        }

        public void RestoreHistoryEvent(UI.HistoryEvent history, bool reopen, ViewStateChangedHistoryEvent viewStateToRestore = null, SelectionEvent selectionEvent = null, bool selectionIsLatent = false)
        {
            (history as History).Restore(this, reopen, viewStateToRestore, selectionEvent, selectionIsLatent);
        }

        void OpenLinkRequest(Database.LinkRequestTable link, bool focus)
        {
            //TODO this code is the same as the one inSpreadsheetPane, should be put together
            //UIElementsHelper.SetVisibility(VisualElements[2], m_ActiveMode.snapshot.NativeAllocationSites.Count > 0 && m_CurrentTableView == TableDisplayMode.Allocations);
            var splitView = VisualElements[2].parent as TwoPaneSplitView;
            if (splitView != null)
            {
                if (m_ActiveMode.snapshot.NativeCallstackSymbols.Count > 0 && m_CurrentTableView == TableDisplayMode.Allocations)
                    splitView.UnCollapse();
                else
                    splitView.CollapseChild(1);
            }

            var tableRef = new Database.TableReference(link.LinkToOpen.TableName, link.Parameters);
            var table = m_ActiveMode.SchemaToDisplay.GetTableByReference(tableRef);
            if (table == null)
            {
                UnityEngine.Debug.LogError("No table named '" + link.LinkToOpen.TableName + "' found.");
                return;
            }
            if (link.LinkToOpen.RowWhere != null && link.LinkToOpen.RowWhere.Count > 0)
            {
                Database.Table filteredTable = table;
                if (table.GetMetaData().defaultFilter != null)
                {
                    filteredTable = table.GetMetaData().defaultFilter.CreateFilter(table);
                }
                Database.Operation.ExpressionParsingContext expressionParsingContext = null;
                if (link.SourceView != null)
                {
                    expressionParsingContext = link.SourceView.ExpressionParsingContext;
                }
                var whereUnion = new Database.View.WhereUnion(link.LinkToOpen.RowWhere, null, null, null, null, m_ActiveMode.SchemaToDisplay, filteredTable, expressionParsingContext);
                var indices = whereUnion.GetMatchingIndices(link.SourceRow);
                var newTab = new Database.Operation.IndexedTable(table, new ArrayRange(indices));
                OpenTable(tableRef, newTab, focus);
            }
            else
            {
                OpenTable(tableRef, table, new Database.CellPosition(0, 0), focus);
            }
        }

        void OnSpreadsheetClick(UI.DatabaseSpreadsheet sheet, Database.LinkRequest link, Database.CellPosition pos)
        {
            if (link.IsPingLink)
            {
                (link as Database.LinkRequestSceneHierarchy).Ping();
                return;
            }

            var tableLinkRequest = link as Database.LinkRequestTable;
            if (tableLinkRequest != null)
            {
                if (tableLinkRequest.LinkToOpen.TableName == ObjectTable.TableName)
                {
                    // TODO: Remove Object table linking, move all details to Details view.
                    // This currently can't be used properly with selection & view History

                    //open object link in the same pane
                    OpenLinkRequest(tableLinkRequest, true);
                    return;
                }
            }
            else
                Debug.LogWarning("Cannot open unknown link '" + link.ToString() + "'");

            //open the link in the spreadsheet pane
            m_EventListener.OnOpenLink(link, m_ActiveMode);
        }

        void OnRowSelected(long rowIndex)
        {
            var snapshotAge = m_UIState.FirstSnapshotAge;
            if (m_ActiveMode != m_UIState.FirstMode)
                snapshotAge = m_UIState.FirstSnapshotAge == SnapshotAge.Newer ? SnapshotAge.Older : SnapshotAge.Newer;

            var selection = new MemorySampleSelection(m_UIState, m_Spreadsheet.DisplayTable, rowIndex, snapshotAge);
            m_UIState.RegisterSelectionChangeEvent(selection);
        }

        void OnUserChangedSpreadsheetFilters()
        {
            var selection = m_UIState.history.GetLastSelectionEvent(MemorySampleSelectionRank.MainSelection);
            if (selection != null && selection.Selection.Valid)
            {
                ApplyActiveSelectionAfterOpening(selection);
            }
        }

        public override void ApplyActiveSelectionAfterOpening(SelectionEvent selectionEvent)
        {
            // TODO: find selected object in Memory Map and frame it
        }

        public override void SetSelectionFromHistoryEvent(SelectionEvent selectionEvent)
        {
            if (selectionEvent.Selection.Rank == MemorySampleSelectionRank.MainSelection)
                m_Spreadsheet.RestoreSelectedRow(selectionEvent.Selection.FindSelectionInTable(m_UIState, m_Spreadsheet.DisplayTable));
            else
            {
                var currentState = m_Spreadsheet.CurrentState;
                currentState.SelectionIsLatent = true;
                m_Spreadsheet.CurrentState = currentState;
                m_EventListener.OnRepaint();
            }
        }

        void OpenTable(Database.TableReference tableRef, Database.Table table, bool focus)
        {
            OpenTable(tableRef, table, Database.CellPosition.invalid, focus);
        }

        void OpenTable(Database.TableReference tableRef, Database.Table table, Database.CellPosition pos, bool focus)
        {
            Filter existingFilter = null;
            var existingColumnCount = 0;
            if (m_Spreadsheet != null)
            {
                existingFilter = m_Spreadsheet.GetCurrentFilterCopy();
                existingColumnCount = m_Spreadsheet.DisplayTable.GetMetaData().GetColumnCount();
            }
            m_Spreadsheet = new UI.DatabaseSpreadsheet(m_UIState.FormattingOptions, table, this);
            m_Spreadsheet.UserChangedFilters += OnUserChangedSpreadsheetFilters;
            m_Spreadsheet.LinkClicked += OnSpreadsheetClick;
            m_Spreadsheet.RowSelectionChanged += OnRowSelected;
            m_Spreadsheet.Goto(pos);
            if (existingFilter != null)
            {
                var state = m_Spreadsheet.CurrentState;
                if (existingColumnCount > 0)
                {
                    var newColumnCount = m_Spreadsheet.DisplayTable.GetMetaData().GetColumnCount();
                    // only copy filters over if the column counts match
                    if (existingColumnCount == newColumnCount)
                        state.Filter = existingFilter;
                }
                m_Spreadsheet.CurrentState = state;
            }

            m_EventListener.OnRepaint();
        }

        void InitializeIfNeeded()
        {
            if (Styles.General == null)
                Styles.Initialize();
        }

        public override void OnGUI(Rect r)
        {
            InitializeIfNeeded();
            if (m_Spreadsheet == null)
                OnSelectRegions(0, 0);

            m_MemoryMap.OnGUI(r);
        }

        void OnGUISpreadsheet(Rect r)
        {
            GUILayout.BeginArea(r);

            int currentTableView = (int)CurrentTableView;

            EditorGUILayout.BeginHorizontal(Styles.MemoryMap.ContentToolbar);

            var age = m_UIState.FirstSnapshotAge;
            if (m_CurrentFirstSnapshotAge != age)
            {
                // first snapshot age changed, recalculate Memory Map
                m_MemoryMap.Setup((m_UIState.FirstMode as UIState.SnapshotMode).snapshot, (m_UIState.SecondMode as UIState.SnapshotMode).snapshot);
                m_CurrentFirstSnapshotAge = age;
                m_UIStateHolder.Repaint();
            }

            if (GUILayout.Toggle(m_ActiveMode == m_UIState.FirstMode, age == SnapshotAge.Older ? "Old (A)" : "New (A)", EditorStyles.radioButton))
            {
                if (m_ActiveMode != m_UIState.FirstMode)
                {
                    m_ActiveMode = m_UIState.FirstMode as UIState.SnapshotMode;
                    m_MemoryMap.Reselect();
                }
            }

            if (GUILayout.Toggle(m_ActiveMode == m_UIState.SecondMode, age == SnapshotAge.Older ? "New (B)" : "Old (B)",  EditorStyles.radioButton))
            {
                if (m_ActiveMode != m_UIState.SecondMode)
                {
                    m_ActiveMode = m_UIState.SecondMode as UIState.SnapshotMode;
                    m_MemoryMap.Reselect();
                }
            }

            if (r.width > 500)
            {
                GUILayout.Space(r.width - 500);
            }

            var popupRect = GUILayoutUtility.GetRect(Content.TableModesList[currentTableView], EditorStyles.toolbarPopup);

            if (EditorGUI.DropdownButton(popupRect, Content.TableModesList[currentTableView], FocusType.Passive, EditorStyles.toolbarPopup))
            {
                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < Content.TableModesList.Length; i++)
                    menu.AddItem(Content.TableModesList[i], (int)currentTableView == i, (object data) => { CurrentTableView = (TableDisplayMode)data; m_MemoryMap.Reselect(); }, i);
                menu.DropDown(popupRect);
            }

            EditorGUILayout.EndHorizontal();

            if (m_Spreadsheet != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                m_Spreadsheet.OnGui_Filters();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(2);
                m_Spreadsheet.OnGUI(r.width - 4);
                GUILayout.Space(2);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }

        void OnGUICallstack(Rect r)
        {
            if (m_Spreadsheet == null) return;
            GUILayout.BeginArea(r);
            if (m_CurrentTableView == TableDisplayMode.Allocations)
            {
                long row = m_Spreadsheet.SelectedRow;

                if (row >= 0)
                {
                    var col = m_Spreadsheet.DisplayTable.GetColumnByName("allocationSiteId");

                    if (col != null && row < col.GetRowCount())
                    {
                        long id = 0;
                        string rowValue = col.GetRowValueString(row, Database.DefaultDataFormatter.Instance);
                        long.TryParse(rowValue, out id);
                        GUI.Label(r, m_ActiveMode.snapshot.NativeAllocationSites.GetReadableCallstackForId(m_ActiveMode.snapshot.NativeCallstackSymbols, id));
                    }
                }
            }
            GUILayout.EndArea();
        }

        void OnGUIToolbarExtension()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            int activeSize = 0;

            for (int i = 0; i < m_BytesInRowList.Length; ++i)
            {
                if (m_BytesInRowList[i].Size == m_MemoryMap.BytesInRow)
                    activeSize = i;
            }
            var popupRect = GUILayoutUtility.GetRect(Content.RowSizeLabel, EditorStyles.toolbarPopup);

            if (EditorGUI.DropdownButton(popupRect, Content.RowSizeLabel, FocusType.Passive, EditorStyles.toolbarPopup))
            {
                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < m_BytesInRowList.Length; i++)
                    menu.AddItem(m_BytesInRowList[i].Content, i == activeSize,  (object data) => m_MemoryMap.BytesInRow = m_BytesInRowList[(int)data].Size, i);
                menu.DropDown(popupRect);
            }

            popupRect = GUILayoutUtility.GetRect(Content.ColorSchemeLabel, EditorStyles.toolbarPopup);

            if (EditorGUI.DropdownButton(popupRect, Content.ColorSchemeLabel, FocusType.Passive, EditorStyles.toolbarPopup))
            {
                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < m_ColorSchemeList.Length; i++)
                {
                    menu.AddItem(m_ColorSchemeList[i], (int)m_MemoryMap.ActiveColorScheme  == i, (object data) => m_MemoryMap.ActiveColorScheme = ((MemoryMap.MemoryMapDiff.ColorScheme)data), i);
                }
                menu.DropDown(popupRect);
            }

            popupRect = GUILayoutUtility.GetRect(Content.FilterLabel, EditorStyles.toolbarPopup);

            if (EditorGUI.DropdownButton(popupRect, Content.FilterLabel, FocusType.Passive, EditorStyles.toolbarPopup))
            {
                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < m_DisplayElementsList.Length; i++)
                {
                    MemoryMap.MemoryMapDiff.DisplayElements element = (MemoryMap.MemoryMapDiff.DisplayElements)(1 << i);
                    menu.AddItem(m_DisplayElementsList[i], m_MemoryMap.ShowDisplayElement(element), (object data) => m_MemoryMap.ToggleDisplayElement((MemoryMap.MemoryMapDiff.DisplayElements)data), element);
                }
                menu.DropDown(popupRect);
            }
            EditorGUILayout.EndHorizontal();
        }

        public override void OnSelectionChanged(MemorySampleSelection selection)
        {
            if (m_Spreadsheet == null)
                return; // Domain Reload or Serialization/Deserialization related untimely event fired. Ignore it, this view is closed for business.

            if (selection.Rank == MemorySampleSelectionRank.SecondarySelection)
                m_Spreadsheet.SetSelectionAsLatent(true);
            switch (selection.Type)
            {
                case MemorySampleSelectionType.NativeObject:
                case MemorySampleSelectionType.ManagedObject:
                case MemorySampleSelectionType.UnifiedObject:
                case MemorySampleSelectionType.Allocation:
                case MemorySampleSelectionType.AllocationSite:
                case MemorySampleSelectionType.Symbol:
                case MemorySampleSelectionType.AllocationCallstack:
                case MemorySampleSelectionType.NativeRegion:
                case MemorySampleSelectionType.ManagedRegion:
                case MemorySampleSelectionType.Allocator:
                    // TODO: check that this is the type of item currently shown and if the selection wasn't made in this view, that it is appropriately updated. For now, assume it was made in this view.
                    break;
                case MemorySampleSelectionType.None:
                case MemorySampleSelectionType.Label:
                case MemorySampleSelectionType.NativeType:
                case MemorySampleSelectionType.ManagedType:
                case MemorySampleSelectionType.Connection:
                case MemorySampleSelectionType.HighlevelBreakdownElement:
                default:
                    if (selection.Rank == MemorySampleSelectionRank.MainSelection)
                        m_Spreadsheet.ClearSelection();
                    break;
            }
        }
    }
}
