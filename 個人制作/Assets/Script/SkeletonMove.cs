//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class SkeletonMove : MonoBehaviour
//{
//    [SerializeField]
//    private EnemySkeleton enemySkeleton;
//    [SerializeField]
//    private SphereCollider searchArea;
//    [SerializeField]
//    private float searchAngle = 130f;

//    private void OnTriggerStay(Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            var playerDirection = other.transform.position - transform.position;

//            var angle = Vector3.Angle(transform.forward, playerDirection);

//            if (angle <= searchAngle)
//            {
//                Debug.Log("主人公を発見1");
//            }
//            else
//            {
//                if (Vector3.Distance(other.transform.position, transform.position) <= searchArea.radius * 0.5f)
//                {
//                    Debug.Log("主人公を発見2");
//                }
//            }
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            enemySkeleton.SetState(SkeletonMove.EnemyState.Wait);
//        }
//    }
//#if UNITY_EDITOR
//    //　サーチする角度表示
//    private void OnDrawGizmos()
//    {
//        Handles.color = Color.red;
//        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
//    }
//#endif
//}