﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagiCloud.KGUI
{
    /// <summary>
    /// 背包子项
    /// </summary>
    public class KGUI_BackpackItem : KGUI_ButtonBase
    {
        private Text txtName, txtNumber;                  //名字，数量
        private Image Icon;                             //图标

        public KGUI_Backpack_ItemData dataConfig { get; private set; }

        private Sprite normalIcon, disableIcon;

        private string _equipmentName;                      //仪器名字
        private int _equipmentNumber;

        public KGUI_Backpack Backpack {
            get; set;
        }

        /// <summary>
        /// 生成的物体
        /// </summary>
        public List<GameObject> GenerateItems { get; set; }

        public void OnInitialized(KGUI_Backpack backpack, KGUI_Backpack_ItemData config)
        {
            dataConfig = config;
            Backpack = backpack;

            txtName = transform.Find("Name").GetComponent<Text>();
            txtNumber = transform.Find("Number").GetComponent<Text>();
            Icon = transform.Find("Icon").GetComponent<Image>();

            ////设置初始值
            //buttonType = ButtonType.Image;
            //image = GetComponent<Image>();

            //normalSprite = Backpack.dataConfig.normalIcon;
            //enterSprite = Backpack.dataConfig.enterIcon;
            //disableSprite = Backpack.dataConfig.disableIcon;

            //获取到路径信息
            //normalIcon = config.NormalSprite;
            //disableIcon = config.DisableSprite;

            normalIcon = backpack.backpackIcons.GetSprite(config.normalSpritePath);
            disableIcon = backpack.backpackIcons.GetSprite(config.disableSpritePath);

            //disableIcon = config.DisableSprite;

            Icon.sprite = normalIcon;

            txtName.text = config.Name;

            if (txtNumber != null)
                txtNumber.text = config.number.ToString();

            _equipmentNumber = config.number;
            _equipmentName = config.Name;

            GenerateItems = new List<GameObject>();

            //刷新
            RefreshShow();
        }

        /// <summary>
        /// 创建仪器
        /// </summary>
        /// <param name="handIndex"></param>
        /// <returns></returns>
        public GameObject CreatEquipment(int handIndex)
        {
            GameObject go = null;

            if (_equipmentNumber >= 1)
            {
                _equipmentNumber--;
                RefreshShow();
                go = Backpack.CreatEquipment(this, handIndex);
            }
            else if (_equipmentNumber == 0)
            {
                //0 不触发 
            }
            else if (_equipmentNumber == -1)
            {
                // -1 数量无限
                go = Backpack.CreatEquipment(this, handIndex);
            }

            if (go != null)
            {
                GenerateItems.Add(go);//将生成的物体添加到子项中
            }

            return go;
        }

        /// <summary>
        /// 删除仪器
        /// </summary>
        public void DestroyEquipment(GameObject target)
        {
            if (target != null)
            {
                if (GenerateItems.Contains(target))
                    GenerateItems.Remove(target);
            }

            if (_equipmentNumber != -1)
            {
                _equipmentNumber++;
                RefreshShow();
            }
        }

        /// <summary>
        /// 刷新显示
        ///     数值变化，图标变换
        /// </summary>
        private void RefreshShow()
        {
            if (Icon == null) return;

            if (_equipmentNumber == -1)
            {
                if (txtNumber != null)
                    //数量无限制 
                    txtNumber.text = "∞";

                Icon.sprite = normalIcon;
            }
            else if (_equipmentNumber == 0)
            {
                if (txtNumber != null)
                    //图标变换成disableIcon，数值变换
                    txtNumber.text = _equipmentNumber.ToString();

                IsEnable = false;

                Icon.sprite = disableIcon;
            }
            else
            {
                if (txtNumber != null)
                    //数值，图标变换
                    txtNumber.text = _equipmentNumber.ToString();

                if (_equipmentNumber > 0)
                    IsEnable = true;

                Icon.sprite = normalIcon;
            }
        }

        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="handIndex"></param>
        public override void OnDown(int handIndex)
        {
            base.OnDown(handIndex);
            if (dataConfig.number == 0) return;

            CreatEquipment(handIndex);
        }

        /// <summary>
        /// 删除已经生成物体的所有仪器
        /// </summary>
        public void OnDestroyItems()
        {
            foreach (var item in GenerateItems)
            {
                Destroy(item.gameObject);
            }

            GenerateItems.Clear();
        }
    }
}
