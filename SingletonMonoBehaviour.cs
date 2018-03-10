
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core
{
    /// <summary>
    /// Cette classe permet avoir une instance unique, donc 
    /// statique d'un MonoBehaviour, lié a un GameObject qui lui, est public. Autrement dit, l'instance
    /// (T Instance) représentera le dernier MonoBehaviour a avoir été instantié (indirectement, 
    /// le dernier GameObject a avoir été instantié).
    /// La subtilité de  cette classe est que le T ici est structuré récursivement , vu qu'il fait reférence à la classe qui hérite de 
    /// SingletonMonoBehaviour<T> 
    /// Donc T = SingletonMonoBehaviour<T> where T : SingletonMonoBehaviour<T>  where T : SingletonMonoBehaviour<T>  where 
    /// T : SingletonMonoBehaviour<T>  .... etc , le principe même d'un singleton.
    /// La seule chose a vérifier : Les performances de la Reflection 
    /// Sans System.Reflection, cette classe n'est pas accépté par le compilateur, car incohérante avec la logique des 
    /// languages  objets. En effet, on ne peut pas écrire : 'Instance = this;' dans Awake() 
    /// 
    /// Utilisation = 'MonScriptUnity.Instance.maFonctionPublique();' ou 'SingletonMonoBehaviour<T>.Instance.maFonctionPublique();'
    /// 
    /// Cette classe est utile dans l'utilisation des UI. (c'est comme ça que je l'ai utilisé du moins)
    /// </summary>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> // wtf <3
    {
        public static T Instance
        {
            get;
            private set;
        }

        private PropertyInfo InstanceProperty;

        public SingletonMonoBehaviour()
        {
            InstanceProperty = this.GetType().BaseType.GetProperty("Instance");
        }
        /// <summary>
        /// Merci System.Reflection ;)
        /// </summary>
        public void Awake()
        {
            InstanceProperty.SetValue(null, this);
        }
        public void OnDestroy()
        {
            Instance = null;
        }

    }
}