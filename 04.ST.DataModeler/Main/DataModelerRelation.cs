using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public partial class DataModeler
    {
        #region SetNodeRelationDrawInfoDic
        private void SetNodeRelationDrawInfoDic(GraphicControl targetControl = null)
        {
            // 현재 RalationControl 내부 OnPaint 메서드에 하단 RalationControl.SetDrawInfo() 를 사용하도록 설정돼 있음

            /*
            List<RelationControl> targetRelations = new List<RelationControl>();

            // Set targetNodeRelations
            List<RelationControl> targetNodeInfoToBeAdded = new List<RelationControl>();
            if (targetControl != null)
            {
                foreach (RelationControl relation in Relations)
                {
                    TableNode tableNode = targetControl as TableNode;
                    if (tableNode != null)
                    {
                        if (relation.Model.NODE_ID1 == tableNode.ID && relation.Model.NODE_SEQ1 == tableNode.SEQ)
                        {
                            targetRelations.Add(relation);
                        }
                        else if (relation.Model.NODE_ID2 == tableNode.ID && relation.Model.NODE_SEQ2 == tableNode.SEQ)
                        {
                            bool rsContains = false;
                            foreach (RelationControl nodeRelation in targetNodeInfoToBeAdded)
                            {
                                if (nodeRelation.Model.NODE_ID1 == relation.Model.NODE_ID1
                                 && nodeRelation.Model.NODE_SEQ1 == relation.Model.NODE_SEQ1)
                                {
                                    rsContains = true;
                                    break;
                                }
                            }
                            if (!rsContains)
                            {
                                targetNodeInfoToBeAdded.Add(relation);
                            }
                        }
                    }
                }

                // targetNodeInfoToBeAdded Proc
                foreach (RelationControl relation in Relations)
                {
                    foreach (RelationControl nodeRelation in targetNodeInfoToBeAdded)
                    {
                        if (nodeRelation.Model.NODE_ID1 == relation.Model.NODE_ID1
                         && nodeRelation.Model.NODE_SEQ1 == relation.Model.NODE_SEQ1)
                        {
                            targetRelations.Add(relation);
                            break;
                        }
                    }
                }
            }
            else
            {
                targetRelations = Relations.ToList();
            }

            // Set NodeRelationDrawInfoDic
            foreach (RelationControl relation in targetRelations)
            {
                relation.SetDrawInfo();
            }
            */
        }
        #endregion
    }
}
