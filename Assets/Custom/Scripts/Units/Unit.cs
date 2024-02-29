using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CMGame.Gameplay
{
    public enum UnitCategory
    {
        Player,
        Bot
    }

    public enum UnitType
    {
        Attack,
        Defence,
        Buff
    }

    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected UnitCategory category;
        [SerializeField] private UnitType type;
        [SerializeField] private int damageValue;
        [SerializeField] private int hpValue;
        [SerializeField] private UnitStatus visuals;
        [SerializeField] private UnitAnimation unitAnim;

        private Transform unitTransform;
        private EventTrigger trigger;

        protected int hp;
        protected int damage;
        protected int actions;

        private Vector3 offScreenPos;
        private bool isEnabled;

        private FieldCell linkedCell;
        private System.Action<Unit> PickCallback;
        private System.Action<Unit> DestroyCallback;

        private void CheckActions()
        {
            if (actions <= 0)
            {
                SwitchUnit(false);
            }
        }

        private void HideUnit()
        {
            unitTransform.position = offScreenPos;
            if (DestroyCallback != null)
            {
                DestroyCallback(this);
            }
        }

        protected void FinishAction()
        {
            unitAnim.PlayAction();
            actions--;
            CheckActions();
        }

        public void Init()
        {
            unitTransform = transform;
            trigger = GetComponent<EventTrigger>();
            visuals.Init();
            unitAnim.Init();
        }

        public void ResetUnit()
        {
            unitAnim.ResetScale(false);
            isEnabled = true;

            hp = hpValue;
            damage = damageValue;
        }

        public void RefreshUnit(int count)
        {
            actions = count;
            damage = Mathf.Clamp(damage, 0, damageValue);
            hp = Mathf.Clamp(hp, 0, hpValue);
            unitAnim.ResetScale(true);
            visuals.SetMaterial(true);
        }

        public void UpdateActions()
        {
            actions++;
        }

        public void UpdateHp()
        {
            hp++;
            unitAnim.ScaleHp();
        }

        public void UpdateDamage()
        {
            damage++;
            unitAnim.ScaleDamage();
        }

        public void Activate()
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { PickUnit((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }

        public void Deactivate()
        {
            trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }

        public void SwitchUnit(bool active)
        {
            trigger.enabled = active;
            visuals.SetMaterial(active);
        }

        public void PickUnit(PointerEventData data)
        {
            if (PickCallback != null)
            {
                PickCallback(this);
            }
        }

        public void DamageUnit(int damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                isEnabled = false;
                unitAnim.DescaleUnit(HideUnit);
            }
        }

        public bool IsUnitEnabled()
        {
            return isEnabled;
        }

        public UnitCategory GetUnitCategory()
        {
            return category;
        }

        public UnitType GetUnitType()
        {
            return type;
        }

        public FieldCell GetUnitCell()
        {
            return linkedCell;
        }

        public void SetPickCallback(System.Action<Unit> callback)
        {
            PickCallback = callback;
        }

        public void SetDestroyCallback(System.Action<Unit> callback, Vector3 hiddenPos)
        {
            DestroyCallback = callback;
            offScreenPos = new Vector3(hiddenPos.x, unitTransform.position.y, hiddenPos.z);
        }

        public void PlaceUnit(FieldCell targetCell, bool animated = false)
        {
            Vector3 targetPos = targetCell.GetCellPosition();
            if (animated)
            {
                unitTransform.DOMove(new Vector3(targetPos.x, unitTransform.position.y, targetPos.z), 0.5f)
                .SetId("game_unit");
            }
            else
            {
                unitTransform.position = new Vector3(targetPos.x, unitTransform.position.y, targetPos.z);
            }
            linkedCell = targetCell;
        }

        public void Move(FieldCell targetCell)
        {
            PlaceUnit(targetCell, true);

            actions--;
            CheckActions();
        }

        public void Attack(Unit targetUnit)
        {
            targetUnit.DamageUnit(damage);
            unitAnim.ShakeScale();

            actions--;
            CheckActions();
        }

        public abstract void Act(List<Unit> targets);
    }
}
