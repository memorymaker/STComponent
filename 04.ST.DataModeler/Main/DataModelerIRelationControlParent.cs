using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class DataModeler : IRelationControlParent
    {
        #region Interface Implementation
        public RelationControlCollection Relations => _Relations;
        private RelationControlCollection _Relations;

        public event RelationControlEventHandler RelationAdded;
        public event RelationControlEventHandler RelationRemoved;

        private void LoadIRelationControlParent()
        {
            _Relations = new RelationControlCollection(this);
        }

        public void OnRelationAdded(RelationControlEventArgs e)
        {
            RelationAdded?.Invoke(this, e);
        }

        public void OnRelationRemoved(RelationControlEventArgs e)
        {
            RelationRemoved?.Invoke(this, e);
        }
        #endregion

        #region Etc Function
        public void AddRelations(Dictionary<string, object> relations)
        {
            var items1 = relations["ItemsOrigin"] as List<GraphicListViewItem>;
            var items2 = relations["ItemsDestination"] as List<GraphicListViewItem>;

            // Column To Column
            if (items2 != null)
            {
                for (int i = 0; i < items1.Count; i++)
                {
                    if (i < items2.Count)
                    {
                        RelationControl relation = new RelationControl(this,
                            new RelationModel()
                            {
                                RELATION_TYPE = GetRelationType(
                                    items1[i].Row["NODE_ID"].ToString()
                                    , Convert.ToInt32(items1[i].Row["NODE_SEQ"])
                                    , items2[i].Row["NODE_ID"].ToString()
                                    , Convert.ToInt32(items2[i].Row["NODE_SEQ"])
                                ),
                                RELATION_OPERATOR = "",
                                RELATION_VALUE = "",
                                RELATION_NOTE = "",
                                NODE_ID1 = items1[i].Row["NODE_ID"].ToString(),
                                NODE_SEQ1 = Convert.ToInt32(items1[i].Row["NODE_SEQ"]),
                                NODE_DETAIL_ID1 = items1[i].Row["NODE_DETAIL_ID"].ToString(),
                                NODE_DETAIL_SEQ1 = Convert.ToInt32(items1[i].Row["NODE_DETAIL_SEQ"]),
                                NODE_DETAIL_ORDER1 = Convert.ToInt32(items1[i].Row["NODE_DETAIL_ORDER"]),
                                NODE_ID2 = items2[i].Row["NODE_ID"].ToString(),
                                NODE_SEQ2 = Convert.ToInt32(items2[i].Row["NODE_SEQ"]),
                                NODE_DETAIL_ID2 = items2[i].Row["NODE_DETAIL_ID"].ToString(),
                                NODE_DETAIL_SEQ2 = Convert.ToInt32(items2[i].Row["NODE_DETAIL_SEQ"]),
                                NODE_DETAIL_ORDER2 = Convert.ToInt32(items2[i].Row["NODE_DETAIL_ORDER"])
                            }
                        );

                        if (!ContainsRelation(relation))
                        {
                            Relations.Add(relation);
                        }
                    }
                }
            }
            // Column To Table
            else
            {
                var itemTable = relations["ItemsDestination"] as RelationModel;
                if (itemTable != null)
                {
                    RelationModel relationModel = null;
                    for (int i = 0; i < items1.Count; i++)
                    {
                        relationModel = new RelationModel()
                        {
                            RELATION_TYPE = itemTable.RELATION_TYPE,
                            RELATION_OPERATOR = itemTable.RELATION_OPERATOR,
                            RELATION_VALUE = itemTable.RELATION_VALUE,
                            RELATION_NOTE = "",
                            NODE_ID1 = items1[i].Row["NODE_ID"].ToString(),
                            NODE_SEQ1 = Convert.ToInt32(items1[i].Row["NODE_SEQ"]),
                            NODE_DETAIL_ID1 = items1[i].Row["NODE_DETAIL_ID"].ToString(),
                            NODE_DETAIL_SEQ1 = Convert.ToInt32(items1[i].Row["NODE_DETAIL_SEQ"]),
                            NODE_DETAIL_ORDER1 = Convert.ToInt32(items1[i].Row["NODE_DETAIL_ORDER"]),
                            NODE_ID2 = itemTable.NODE_ID2,
                            NODE_SEQ2 = itemTable.NODE_SEQ2,
                            NODE_DETAIL_ID2 = string.Empty,
                            NODE_DETAIL_SEQ2 = 0,
                            NODE_DETAIL_ORDER2 = 0
                        };

                        RelationControl relation = new RelationControl(this, relationModel);
                        if (!ContainsRelation(relation))
                        {
                            Relations.Add(relation);
                        }
                    }

                    if (relationModel != null)
                    {
                        foreach (RelationControl control in Relations)
                        {
                            if (!control.Model.Equals(relationModel)
                            && control.Model.NODE_ID1 == relationModel.NODE_ID1
                            && control.Model.NODE_SEQ1 == relationModel.NODE_SEQ1
                            && control.Model.NODE_ID2 == relationModel.NODE_ID2
                            && control.Model.NODE_SEQ2 == relationModel.NODE_SEQ2)
                            {
                                control.Model.RELATION_TYPE = relationModel.RELATION_TYPE;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Type of the items2 is incorrect.");
                }
            }

            SetNodeRelationDrawInfoDic();
        }

        public string GetRelationType(string NODE_ID1, int NODE_SEQ1, string NODE_ID2, int NODE_SEQ2)
        {
            string rs = RelationControlType.InnerJoin.GetStringValue();
            for(int i = 0; i < Relations.Count; i++)
            {
                if (Relations[i].Model.NODE_ID1 == NODE_ID1
                && Relations[i].Model.NODE_SEQ1 == NODE_SEQ1
                && Relations[i].Model.NODE_ID2 == NODE_ID2
                && Relations[i].Model.NODE_SEQ2 == NODE_SEQ2
                )
                {
                    rs = Relations[i].Model.RELATION_TYPE;
                    break;
                }
            }
            return rs;
        }

        public void RemoveRelations(List<RelationControl> relationControls)
        {
            for (int i = 0; i < relationControls.Count; i++)
            {
                Relations.Remove(relationControls[i]);
            }
        }

        private bool ContainsRelation(RelationControl relationControl)
        {
            bool rsContains = false;
            foreach (RelationControl nodeRelation in Relations)
            {
                if (nodeRelation.Model.NODE_ID1        == relationControl.Model.NODE_ID1
                && nodeRelation.Model.NODE_SEQ1        == relationControl.Model.NODE_SEQ1
                && nodeRelation.Model.NODE_DETAIL_ID1  == relationControl.Model.NODE_DETAIL_ID1
                && nodeRelation.Model.NODE_DETAIL_SEQ1 == relationControl.Model.NODE_DETAIL_SEQ1
                && nodeRelation.Model.NODE_ID2         == relationControl.Model.NODE_ID2
                && nodeRelation.Model.NODE_SEQ2        == relationControl.Model.NODE_SEQ2
                && nodeRelation.Model.NODE_DETAIL_ID2  == relationControl.Model.NODE_DETAIL_ID2
                && nodeRelation.Model.NODE_DETAIL_SEQ2 == relationControl.Model.NODE_DETAIL_SEQ2)
                {
                    rsContains = true;
                    break;
                }
            }
            return rsContains;
        }

        public bool ContainsRelation(RelationModel relationModel)
        {
            bool rsContains = false;
            foreach (RelationControl nodeRelation in Relations)
            {
                if (nodeRelation.Model.NODE_ID1        == relationModel.NODE_ID1
                && nodeRelation.Model.NODE_SEQ1        == relationModel.NODE_SEQ1
                && nodeRelation.Model.NODE_DETAIL_ID1  == relationModel.NODE_DETAIL_ID1
                && nodeRelation.Model.NODE_DETAIL_SEQ1 == relationModel.NODE_DETAIL_SEQ1
                && nodeRelation.Model.NODE_ID2         == relationModel.NODE_ID2
                && nodeRelation.Model.NODE_SEQ2        == relationModel.NODE_SEQ2
                && nodeRelation.Model.NODE_DETAIL_ID2  == relationModel.NODE_DETAIL_ID2
                && nodeRelation.Model.NODE_DETAIL_SEQ2 == relationModel.NODE_DETAIL_SEQ2)
                {
                    rsContains = true;
                    break;
                }
            }
            return rsContains;
        }
        #endregion
    }
}