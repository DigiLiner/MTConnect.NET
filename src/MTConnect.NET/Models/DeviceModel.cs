// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect.Assets;
using MTConnect.Devices;
using MTConnect.Devices.Components;
using MTConnect.Devices.Events;
using MTConnect.Models.Components;
using MTConnect.Models.DataItems;
using MTConnect.Observations;
using MTConnect.Streams;
using MTConnect.Streams.Events;
using MTConnect.Streams.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MTConnect.Models
{
    /// <summary>
    /// MTConnect Device Model used to access MTConnectDevices, MTConnectSreams, and MTConnectAssets data in a single object
    /// </summary>
    public class DeviceModel : Device, IDeviceModel
    {
        private readonly object _lock = new object();
        private readonly Dictionary<string, object> _dataItems = new Dictionary<string, object>();
        private readonly Dictionary<string, Condition> _conditions = new Dictionary<string, Condition>();
        private readonly Dictionary<string, IAsset> _assets = new Dictionary<string, IAsset>();


        /// <summary>
        /// The name of the manufacturer of the Component
        /// </summary>
        public string Manufacturer
        {
            get => base.Description?.Manufacturer;
            set
            {
                if (base.Description == null) base.Description = new Description();
                base.Description.Manufacturer = value;
            }
        }

        /// <summary>
        /// The model description of the Component
        /// </summary>
        public string Model
        {
            get => base.Description?.Model;
            set
            {
                if (base.Description == null) base.Description = new Description();
                base.Description.Model = value;
            }
        }

        /// <summary>
        /// The component's serial number
        /// </summary>
        public string SerialNumber
        {
            get => base.Description?.SerialNumber;
            set
            {
                if (base.Description == null) base.Description = new Description();
                base.Description.SerialNumber = value;
            }
        }

        /// <summary>
        /// The station where the Component is located when a component is part of a manufacturing unit or cell with multiple stations that share the same physical controller.
        /// </summary>
        public string Station
        {
            get => base.Description?.Station;
            set
            {
                if (base.Description == null) base.Description = new Description();
                base.Description.Station = value;
            }
        }

        /// <summary>
        /// Any additional descriptive information the implementer chooses to include regarding the Component.
        /// </summary>
        public new string Description
        {
            get => base.Description?.CDATA;
            set
            {
                if (base.Description == null) base.Description = new Description();
                base.Description.CDATA = value;
            }
        }


        /// <summary>
        /// List of Child ComponentModels 
        /// </summary>
        public List<IComponentModel> ComponentModels { get; set; }

        public override List<Component> Components
        {
            get
            {
                var x = new List<Component>();

                if (!ComponentModels.IsNullOrEmpty())
                {
                    foreach (var componentModel in ComponentModels.OfType<ComponentModel>()) x.Add(componentModel);
                }

                return x;
            }
        }


        /// <summary>
        /// List of Child CompositionModels 
        /// </summary>
        public List<ICompositionModel> CompositionModels { get; set; }

        public override List<Composition> Compositions
        {
            get
            {
                var x = new List<Composition>();

                if (!CompositionModels.IsNullOrEmpty())
                {
                    foreach (var compositionModel in CompositionModels.OfType<CompositionModel>()) x.Add(compositionModel);
                }

                return x;
            }
        }


        /// <summary>
        /// List of Child DataItemModels 
        /// </summary>
        public List<IDataItemModel> DataItemModels { get; set; }

        public override List<Devices.DataItem> DataItems
        {
            get
            {
                var x = new List<Devices.DataItem>();

                if (!DataItemModels.IsNullOrEmpty())
                {
                    foreach (var dataItemModel in DataItemModels.OfType<DataItemModel>()) x.Add(dataItemModel);
                }

                return x;
            }
        }


        /// <summary>
        /// Represents the Agent’s ability to communicate with the data source.
        /// </summary>
        public Availability Availability
        {
            get => GetDataItemValue<Availability>(Devices.DataItem.CreateId(Id, Devices.Events.AvailabilityDataItem.NameId));
            set => AddDataItem(new AvailabilityDataItem(Id), value);
        }
        public IDataItemModel AvailabilityDataItem => GetDataItem(Devices.Events.AvailabilityDataItem.NameId);


        /// <summary>
        /// The reference version of the MTConnect Standard supported by the Adapter.
        /// </summary>
        public override Version MTConnectVersion
        {
            get
            {
                var versionString = GetStringValue(Devices.DataItem.CreateId(Id, Devices.Events.MTConnectVersionDataItem.NameId));
                if (!string.IsNullOrEmpty(versionString))
                {
                    if (Version.TryParse(versionString, out var version))
                    {
                        return version;
                    }
                }
                return null;
            }
            set => AddDataItem(new MTConnectVersionDataItem(Id), value);
        }
        public IDataItemModel MTConnectVersionDataItem => GetDataItem(Devices.Events.MTConnectVersionDataItem.NameId);

        /// <summary>
        /// Network details of a component.
        /// </summary>
        public NetworkModel Network
        {
            get => GetNetwork();
            set => SetNetwork(value);
        }

        /// <summary>
        /// The Operating System of a component.
        /// </summary>
        public OperatingSystemModel OperatingSystem
        {
            get => GetOperatingSystem();
            set => SetOperatingSystem(value);
        }

        /// <summary>
        /// Axis is an abstract Component that represents linear or rotational motion for a piece of equipment.
        /// </summary>
        public IAxesModel Axes => GetComponentModel<AxesModel>(typeof(AxesComponent));

        /// <summary>
        /// Controller represents the computational regulation and management function of a piece of equipment.
        /// </summary>
        public IControllerModel Controller => GetComponentModel<ControllerModel>(typeof(ControllerComponent));

        /// <summary>
        /// Systems organizes System component types
        /// </summary>
        public ISystemsModel Systems => GetComponentModel<SystemsModel>(typeof(SystemsComponent));

        /// <summary>
        /// Auxiliaries organizes Auxiliary component types.
        /// </summary>
        public IAuxiliariesModel Auxiliaries => GetComponentModel<AuxiliariesModel>(typeof(AuxiliariesComponent));


        public DeviceModel()
        {
            DataItems = new List<Devices.DataItem>();
            DataItemModels = new List<IDataItemModel>();
            Components = new List<Component>();
            ComponentModels = new List<IComponentModel>();
            Compositions = new List<Composition>();
            CompositionModels = new List<ICompositionModel>();
        }

        public DeviceModel(string deviceName, string deviceId = "dev")
        {
            Id = deviceId;
            Name = deviceName;
            Uuid = deviceId;

            DataItems = new List<Devices.DataItem>();
            DataItemModels = new List<IDataItemModel>();
            Components = new List<Component>();
            ComponentModels = new List<IComponentModel>();
            Compositions = new List<Composition>();
            CompositionModels = new List<ICompositionModel>();
        }

        public DeviceModel(Device device)
        {
            if (device != null)
            {
                Id = device.Id;
                Name = device.Name;
                Uuid = device.Uuid;

                if (device.Description != null)
                {
                    Manufacturer = device.Description.Manufacturer;
                    Model = device.Description.Model;
                    SerialNumber = device.Description.SerialNumber;
                    Station = device.Description.Station;
                    Description = device.Description.CDATA;
                }

                DataItems = new List<Devices.DataItem>();
                DataItemModels = new List<IDataItemModel>();
                Components = new List<Component>();
                ComponentModels = new List<IComponentModel>();
                Compositions = new List<Composition>();
                CompositionModels = new List<ICompositionModel>();

                Initialize(device);
            }
        }


        private void Initialize(Device device)
        {
            if (device != null)
            {
                DataItemModels = CreateDataItemModels(device.DataItems);
                CompositionModels = CreateCompositionModels(device.Compositions);
                ComponentModels = CreateComponentModels(device.Components);
            }
        }


        #region "Create"

        private List<IComponentModel> CreateComponentModels(IEnumerable<Component> components)
        {
            if (!components.IsNullOrEmpty())
            {
                var objs = new List<IComponentModel>();

                foreach (var component in components)
                {
                    var obj = ComponentModel.Create(component);
                    if (obj != null) objs.Add(obj);
                }

                return objs;
            }

            return new List<IComponentModel>();
        }

        private List<ICompositionModel> CreateCompositionModels(IEnumerable<Composition> compositions)
        {
            if (!compositions.IsNullOrEmpty())
            {
                var objs = new List<ICompositionModel>();

                foreach (var composition in compositions)
                {
                    var obj = CompositionModel.Create(composition);
                    if (obj != null) objs.Add(obj);
                }

                return objs;
            }

            return new List<ICompositionModel>();
        }

        private List<IDataItemModel> CreateDataItemModels(IEnumerable<Devices.DataItem> dataItems)
        {
            if (!dataItems.IsNullOrEmpty())
            {
                var objs = new List<IDataItemModel>();

                foreach (var dataItem in dataItems)
                {
                    objs.Add(new DataItemModel(dataItem));
                }

                return objs;
            }

            return new List<IDataItemModel>();
        }

        #endregion

        #region "Update"

        public void UpdateDeviceStream(DeviceStream stream)
        {
            if (stream != null)
            {
                // Add DataItems
                if (!DataItemModels.IsNullOrEmpty())
                {
                    foreach (var dataItem in DataItemModels)
                    {
                        if (dataItem.DataItemCategory == DataItemCategory.CONDITION)
                        {
                            var obj = stream.Conditions.FirstOrDefault(o => o.DataItemId == dataItem.Id);
                            if (obj != null) AddCondition(dataItem, obj);
                        }
                        else
                        {
                            var obj = stream.DataItems.FirstOrDefault(o => o.DataItemId == dataItem.Id);
                            if (obj != null) AddDataItem(dataItem, obj.CDATA);
                        }
                    }
                }

                // Add Compositions
                if (!CompositionModels.IsNullOrEmpty())
                {
                    foreach (var composition in CompositionModels.OfType<CompositionModel>())
                    {
                        UpdateCompositionStream(composition, stream);
                    }
                }

                // Add Components
                if (!ComponentModels.IsNullOrEmpty())
                {
                    foreach (var component in ComponentModels.OfType<ComponentModel>())
                    {
                        UpdateComponentStream(component, stream);
                    }
                }
            }
        }

        private void UpdateComponentStream(ComponentModel component, DeviceStream stream)
        {
            if (component != null && stream != null && !stream.DataItems.IsNullOrEmpty())
            {
                if (!component.DataItemModels.IsNullOrEmpty())
                {
                    foreach (var dataItem in component.DataItemModels)
                    {
                        if (dataItem.DataItemCategory == DataItemCategory.CONDITION)
                        {
                            var obj = stream.Conditions.FirstOrDefault(o => o.DataItemId == dataItem.Id);
                            if (obj != null) component.AddCondition(dataItem, obj);
                        }
                        else
                        {
                            var obj = stream.DataItems.FirstOrDefault(o => o.DataItemId == dataItem.Id);
                            if (obj != null) component.AddDataItem(dataItem, obj.CDATA);
                        }
                    }
                }

                // Add Compositions
                if (!component.CompositionModels.IsNullOrEmpty())
                {
                    foreach (var composition in component.CompositionModels.OfType<CompositionModel>())
                    {
                        UpdateCompositionStream(composition, stream);
                    }
                }

                // Add Components
                if (!component.ComponentModels.IsNullOrEmpty())
                {
                    foreach (var subcomponent in component.ComponentModels.OfType<ComponentModel>())
                    {
                        UpdateComponentStream(subcomponent, stream);
                    }
                }
            }
        }

        private void UpdateCompositionStream(CompositionModel composition, DeviceStream stream)
        {
            if (composition != null && !composition.DataItems.IsNullOrEmpty() && stream != null && !stream.DataItems.IsNullOrEmpty())
            {
                foreach (var dataItem in composition.DataItemModels)
                {
                    if (dataItem.DataItemCategory == DataItemCategory.CONDITION)
                    {
                        var obj = stream.Conditions.FirstOrDefault(o => o.DataItemId == dataItem.Id);
                        if (obj != null) composition.AddCondition(dataItem, obj);
                    }
                    else
                    {
                        var obj = stream.DataItems.FirstOrDefault(o => o.DataItemId == dataItem.Id);
                        if (obj != null) composition.AddDataItem(dataItem, obj.CDATA);
                    }
                }
            }
        }

        #endregion

        #region "Components"

        protected T GetComponentModel<T>(Type componentType, string name = null) where T : ComponentModel
        {
            // Get the TypeId for the ComponentModel Type
            var typeId = GetComponentTypeId(componentType);

            // Find the ComponentModel that matches the TypeId
            var obj = (T)ComponentModels?.FirstOrDefault(o => o.Type == typeId && (name == null || o.Name == name));
            if (obj == null)
            {
                if (name == null)
                {
                    // Get the NameId for the ComponentModel Type
                    name = GetComponentNameId(componentType);
                }

                // If Not found then add a new component using the Type information
                obj = AddComponentModel<T>(name);
            }

            return obj;
        }

        public void AddComponentModel(IComponentModel component)
        {
            if (component != null)
            {
                if (ComponentModels == null) ComponentModels = new List<IComponentModel>();
                ComponentModels.Add(component);
            }
        }

        public T AddComponentModel<T>(string name) where T : ComponentModel
        {
            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    var model = (T)Activator.CreateInstance(typeof(T));
                    model.Id = Component.CreateId(Id, name);
                    model.Name = name;

                    AddComponentModel(model);
                    return model;
                }
                catch { }
            }

            return null;
        }






        public virtual CartesianCoordinateAxesModel AddAxes(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var model = new CartesianCoordinateAxesModel
                {
                    Id = Component.CreateId(Id, name),
                    Name = name
                };

                AddComponentModel(model);
                return model;
            }

            return null;
        }


        public virtual ControllerModel AddController(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var model = new ControllerModel
                {
                    Id = Component.CreateId(Id, name),
                    Name = name
                };

                AddComponentModel(model);
                return model;
            }

            return null;
        }


        public T GetController<T>() where T : ControllerModel
        {
            return (T)Controller;
        }


        public virtual SystemsModel AddSystems(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var model = new SystemsModel
                {
                    Id = Component.CreateId(Id, name),
                    Name = name
                };

                AddComponentModel(model);
                return model;
            }

            return null;
        }


        public virtual AuxiliariesModel AddAuxiliaries(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var model = new AuxiliariesModel
                {
                    Id = Component.CreateId(Id, name),
                    Name = name
                };

                AddComponentModel(model);
                return model;
            }

            return null;
        }

        #endregion

        #region "DataItems"

        protected bool DataItemValueExists(string dataItemId)
        {
            if (!string.IsNullOrEmpty(dataItemId))
            {
                lock (_lock)
                {
                    return _dataItems.TryGetValue(dataItemId, out _);
                }
            }

            return false;
        }


        protected string GetDataItemValue(string dataItemId)
        {
            if (!string.IsNullOrEmpty(dataItemId))
            {
                lock (_lock)
                {
                    if (_dataItems.TryGetValue(dataItemId, out var value) && value != null)
                    {
                        return value.ToString();
                    }
                }
            }

            return default;
        }

        protected T GetDataItemValue<T>(string dataItemId) where T : struct
        {
            if (!string.IsNullOrEmpty(dataItemId))
            {
                lock (_lock)
                {
                    if (_dataItems.TryGetValue(dataItemId, out var value) && value != null)
                    {
                        return StringFunctions.ConvertEnum<T>(value.ToString());
                    }
                }
            }

            return default;
        }


        protected string GetStringValue(string name)
        {
            var dataItemId = Devices.DataItem.CreateId(Id, name);
            if (DataItemValueExists(dataItemId))
            {
                return GetDataItemValue(dataItemId);
            }

            return default;
        }

        protected string GetStringValue(string name, string suffix)
        {
            var dataItemId = Devices.DataItem.CreateId(Id, name, suffix);
            if (DataItemValueExists(dataItemId))
            {
                return GetDataItemValue(dataItemId);
            }

            return default;
        }


        protected EventValue GetEventValue(string name)
        {
            var dataItemId = Devices.DataItem.CreateId(Id, name);
            if (DataItemValueExists(dataItemId))
            {
                return new EventValue(GetDataItemValue(dataItemId));
            }

            return default;
        }

        protected EventValue GetEventValue(string name, string suffix)
        {
            var dataItemId = Devices.DataItem.CreateId(Id, name, suffix);
            if (DataItemValueExists(dataItemId))
            {
                return new EventValue(GetDataItemValue(dataItemId));
            }

            return default;
        }


        protected SampleValue GetSampleValue(string name)
        {
            var dataItemId = Devices.DataItem.CreateId(Id, name);
            if (DataItemValueExists(dataItemId))
            {
                return new SampleValue(GetDataItemValue<double>(dataItemId));
            }

            return default;
        }

        protected SampleValue GetSampleValue(string name, string suffix)
        {
            var dataItemId = Devices.DataItem.CreateId(Id, name, suffix);
            if (DataItemValueExists(dataItemId))
            {
                return new SampleValue(GetDataItemValue<double>(dataItemId));
            }

            return default;
        }


        protected void UpdateDataItem(string dataItemId, object cdata)
        {
            if (!string.IsNullOrEmpty(dataItemId) && cdata != null)
            {
                lock (_lock)
                {
                    _dataItems.Remove(dataItemId);
                    _dataItems.Add(dataItemId, cdata);
                }
            }
        }


        public void AddDataItem(IDataItemModel dataItem, object value)
        {
            if (dataItem != null && !string.IsNullOrEmpty(dataItem.Id) && value != null)
            {
                if (!DataItemModels.Any(o => o.Id == dataItem.Id))
                {
                    DataItemModels.Add(dataItem);
                }

                UpdateDataItem(dataItem.Id, value);
            }
        }

        public void AddDataItem(Devices.DataItem dataItem, object value)
        {
            if (dataItem != null && !string.IsNullOrEmpty(dataItem.Id) && value != null)
            {
                if (!DataItemModels.Any(o => o.Id == dataItem.Id))
                {
                    DataItemModels.Add(new DataItemModel(dataItem));
                }

                UpdateDataItem(dataItem.Id, value);
            }
        }


        protected IDataItemModel GetDataItem(string name, string suffix = null)
        {
            if (!string.IsNullOrEmpty(name) && !DataItemModels.IsNullOrEmpty())
            {
                IDataItemModel dataItem = null;

                if (!string.IsNullOrEmpty(suffix))
                {
                    dataItem = DataItemModels.FirstOrDefault(o => o.Id == Devices.DataItem.CreateId(Id, name, suffix));
                }
                else
                {
                    dataItem = DataItemModels.FirstOrDefault(o => o.Id == Devices.DataItem.CreateId(Id, name));
                }

                return dataItem;
            }

            return default;
        }

        protected T GetDataItem<T>(string name, string suffix = null) where T : IDataItemModel
        {
            if (!string.IsNullOrEmpty(name) && !DataItems.IsNullOrEmpty())
            {
                IDataItemModel dataItem = null;

                if (!string.IsNullOrEmpty(suffix))
                {
                    dataItem = DataItemModels.FirstOrDefault(o => o.Id == Devices.DataItem.CreateId(Id, name, suffix));
                }
                else
                {
                    dataItem = DataItemModels.FirstOrDefault(o => o.Id == Devices.DataItem.CreateId(Id, name));
                }

                if (dataItem != null)
                {
                    try
                    {
                        var obj = (T)Activator.CreateInstance(typeof(T));
                        obj.DataItemCategory = dataItem.DataItemCategory;
                        obj.Id = dataItem.Id;
                        obj.Name = dataItem.Name;
                        obj.Type = dataItem.Type;
                        obj.SubType = dataItem.SubType;
                        obj.NativeUnits = dataItem.NativeUnits;
                        obj.NativeScale = dataItem.NativeScale;
                        obj.SampleRate = dataItem.SampleRate;
                        //obj.Source = dataItem.Source;
                        //obj.Relationships = dataItem.Relationships;
                        obj.Representation = dataItem.Representation;
                        obj.ResetTrigger = dataItem.ResetTrigger;
                        obj.CoordinateSystem = dataItem.CoordinateSystem;
                        //obj.Constraints = dataItem.Constraints;
                        //obj.Definition = dataItem.Definition;
                        obj.Units = dataItem.Units;
                        obj.Statistic = dataItem.Statistic;
                        obj.SignificantDigits = dataItem.SignificantDigits;
                        //obj.Filters = dataItem.Filters;
                        obj.InitialValue = dataItem.InitialValue;

                        return obj;
                    }
                    catch { }
                }
            }

            return default;
        }

        #endregion

        #region "Conditions"

        protected Condition GetCondition(string name)
        {
            var dataItemId = Devices.DataItem.CreateId(Id, name);
            if (ConditionValueExists(dataItemId))
            {
                lock (_lock)
                {
                    if (_conditions.TryGetValue(dataItemId, out var condition))
                    {
                        return condition;
                    }
                }
            }

            return default;
        }

        protected bool ConditionValueExists(string dataItemId)
        {
            if (!string.IsNullOrEmpty(dataItemId))
            {
                lock (_lock)
                {
                    return _conditions.TryGetValue(dataItemId, out _);
                }
            }

            return false;
        }


        protected void UpdateCondition(string dataItemId, Condition condition)
        {
            if (!string.IsNullOrEmpty(dataItemId) && condition != null)
            {
                lock (_lock)
                {
                    _conditions.Remove(dataItemId);
                    _conditions.Add(dataItemId, condition);
                }
            }
        }



        public void AddCondition(Devices.DataItem dataItem, Condition condition)
        {
            if (dataItem != null && !string.IsNullOrEmpty(dataItem.Id) && condition != null)
            {
                if (!DataItemModels.Any(o => o.Id == dataItem.Id))
                {
                    DataItemModels.Add(new DataItemModel(dataItem));
                }

                UpdateCondition(dataItem.Id, condition);
            }
        }

        public void AddCondition(IDataItemModel dataItem, Condition condition)
        {
            if (dataItem != null && !string.IsNullOrEmpty(dataItem.Id) && condition != null)
            {
                if (!DataItemModels.Any(o => o.Id == dataItem.Id))
                {
                    DataItemModels.Add(dataItem);
                }

                UpdateCondition(dataItem.Id, condition);
            }
        }

        #endregion

        #region "Assets"

        public IAsset GetAsset(string assetId)
        {
            if (!string.IsNullOrEmpty(assetId))
            {
                lock (_lock)
                {
                    if (_assets.TryGetValue(assetId, out var asset))
                    {
                        return asset;
                    }
                }
            }

            return default;
        }

        public T GetAsset<T>(string assetId) where T: IAsset
        {
            if (!string.IsNullOrEmpty(assetId))
            {
                lock (_lock)
                {
                    if (_assets.TryGetValue(assetId, out IAsset asset))
                    {
                        return (T)asset;
                    }
                }
            }

            return default;
        }

        public IEnumerable<IAsset> GetAssets()
        {
            lock (_lock)
            {
                if (!_assets.IsNullOrEmpty())
                {
                    return _assets.Values.ToList();
                }
            }

            return default;
        }

        public IEnumerable<MTConnect.Assets.CuttingTools.CuttingToolAsset> GetCuttingTools()
        {
            lock (_lock)
            {
                if (!_assets.IsNullOrEmpty())
                {
                    var cuttingToolAssets = _assets.Values.Where(o => o.Type == MTConnect.Assets.CuttingTools.CuttingToolAsset.TypeId);
                    if (!cuttingToolAssets.IsNullOrEmpty())
                    {
                        var cuttingTools = new List<MTConnect.Assets.CuttingTools.CuttingToolAsset>();
                        foreach (var asset in cuttingToolAssets)
                        {
                            cuttingTools.Add((MTConnect.Assets.CuttingTools.CuttingToolAsset)asset);
                        }
                        return cuttingTools;
                    }
                }
            }

            return default;
        }


        protected bool AssetExists(string assetId)
        {
            if (!string.IsNullOrEmpty(assetId))
            {
                lock (_lock)
                {
                    return _assets.TryGetValue(assetId, out _);
                }
            }

            return false;
        }

        protected void UpdateAsset(string assetId, IAsset asset)
        {
            if (!string.IsNullOrEmpty(assetId) && asset != null)
            {
                lock (_lock)
                {
                    _assets.Remove(assetId);
                    _assets.Add(assetId, asset);
                }
            }
        }


        public void AddAsset(IAsset asset)
        {
            if (asset != null)
            {
                UpdateAsset(asset.AssetId, asset);
            }
        }

        #endregion

        #region "Adapter"

        public IEnumerable<Observation> GetObservations(long timestamp = 0)
        {
            var objs = new List<Observation>();

            if (timestamp <= 0) timestamp = UnixDateTime.Now;

            lock (_lock)
            {
                foreach (var dataItem in _dataItems.ToList())
                {
                    var obj = new Observation(dataItem.Key, dataItem.Value);
                    obj.Timestamp = timestamp;
                    objs.Add(obj);
                }
            }

            if (!CompositionModels.IsNullOrEmpty())
            {
                foreach (CompositionModel compositionModel in CompositionModels.OfType<CompositionModel>())
                //foreach (CompositionModel compositionModel in CompositionModels)
                {
                    objs.AddRange(compositionModel.GetObservations(timestamp));
                }
            }

            if (!ComponentModels.IsNullOrEmpty())
            {
                foreach (var componentModel in ComponentModels.OfType<ComponentModel>())
                //foreach (ComponentModel componentModel in ComponentModels)
                {
                    objs.AddRange(componentModel.GetObservations(timestamp));
                }
            }

            return objs;
        }

        public IEnumerable<ConditionObservation> GetConditionObservations(long timestamp = 0)
        {
            var objs = new List<ConditionObservation>();

            if (timestamp <= 0) timestamp = UnixDateTime.Now;

            lock (_lock)
            {
                foreach (var condition in _conditions.ToList())
                {
                    var obj = new ConditionObservation(condition.Key, condition.Value.Level);
                    obj.NativeCode = condition.Value.NativeCode;
                    obj.NativeSeverity = condition.Value.NativeSeverity;
                    obj.Qualifier = condition.Value.Qualifier;
                    obj.Message = condition.Value.CDATA;
                    obj.Timestamp = timestamp;
                    objs.Add(obj);
                }
            }

            if (!CompositionModels.IsNullOrEmpty())
            {
                foreach (var compositionModel in CompositionModels.OfType<CompositionModel>())
                {
                    objs.AddRange(compositionModel.GetConditionObservations(timestamp));
                }
            }

            if (!ComponentModels.IsNullOrEmpty())
            {
                foreach (var componentModel in ComponentModels.OfType<ComponentModel>())
                {
                    objs.AddRange(componentModel.GetConditionObservations(timestamp));
                }
            }

            return objs;
        }

        public IEnumerable<IAsset> GetAdapterAssets()
        {
            var objs = new List<IAsset>();

            lock (_lock)
            {
                foreach (var asset in _assets.ToList())
                {
                    objs.Add(asset.Value);
                }
            }

            return objs;
        }

        #endregion


        #region "Network"

        private NetworkModel GetNetwork()
        {
            var x = new NetworkModel();

            x.IPv4Address = GetStringValue(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.IPV4_ADDRESS));
            x.IPv4AddressDataItem = GetDataItem(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.IPV4_ADDRESS));

            x.IPv6Address = GetStringValue(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.IPV6_ADDRESS));
            x.IPv6AddressDataItem = GetDataItem(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.IPV6_ADDRESS));

            x.Gateway = GetStringValue(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.GATEWAY));
            x.GatewayDataItem = GetDataItem(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.GATEWAY));

            x.SubnetMask = GetStringValue(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.SUBNET_MASK));
            x.SubnetMaskDataItem = GetDataItem(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.SUBNET_MASK));

            x.MacAddress = GetStringValue(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.MAC_ADDRESS));
            x.MacAddressDataItem = GetDataItem(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.MAC_ADDRESS));

            x.VLanId = GetStringValue(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.VLAN_ID));
            x.VLanIdDataItem = GetDataItem(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.VLAN_ID));

            x.Wireless = GetStringValue(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.WIRELESS));
            x.WirelessDataItem = GetDataItem(NetworkDataItem.NameId, NetworkDataItem.GetSubTypeId(NetworkDataItem.SubTypes.WIRELESS));

            return x;

        }

        private void SetNetwork(NetworkModel network)
        {
            if (network != null)
            {
                AddDataItem(new NetworkDataItem(Id, NetworkDataItem.SubTypes.IPV4_ADDRESS), network.IPv4Address);
                AddDataItem(new NetworkDataItem(Id, NetworkDataItem.SubTypes.IPV6_ADDRESS), network.IPv6Address);
                AddDataItem(new NetworkDataItem(Id, NetworkDataItem.SubTypes.GATEWAY), network.Gateway);
                AddDataItem(new NetworkDataItem(Id, NetworkDataItem.SubTypes.SUBNET_MASK), network.SubnetMask);
                AddDataItem(new NetworkDataItem(Id, NetworkDataItem.SubTypes.VLAN_ID), network.VLanId);
                AddDataItem(new NetworkDataItem(Id, NetworkDataItem.SubTypes.WIRELESS), network.Wireless);
            }
        }

        #endregion

        #region "Operating System"

        private OperatingSystemModel GetOperatingSystem()
        {
            var x = new OperatingSystemModel();

            x.Type = GetDataItemValue<OperatingSystemType>(Devices.DataItem.CreateId(Id, OperatingSystemDataItem.NameId));
            x.TypeDataItem = GetDataItem(OperatingSystemDataItem.NameId);

            x.Version = GetStringValue(OperatingSystemDataItem.NameId, OperatingSystemDataItem.GetSubTypeId(OperatingSystemDataItem.SubTypes.VERSION));
            x.VersionDataItem = GetDataItem(OperatingSystemDataItem.NameId, OperatingSystemDataItem.GetSubTypeId(OperatingSystemDataItem.SubTypes.VERSION));

            return x;

        }

        private void SetOperatingSystem(OperatingSystemModel operatingSystem)
        {
            if (operatingSystem != null)
            {
                AddDataItem(new OperatingSystemDataItem(Id), operatingSystem.Type);
                AddDataItem(new OperatingSystemDataItem(Id, OperatingSystemDataItem.SubTypes.VERSION), operatingSystem.Version);
            }
        }

        #endregion


        private string GetComponentTypeId(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (!fieldInfos.IsNullOrEmpty())
            {
                var constants = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
                if (!constants.IsNullOrEmpty())
                {
                    var fieldInfo = constants.FirstOrDefault(o => o.Name == "TypeId");
                    if (fieldInfo != null)
                    {
                        try
                        {
                            var obj = (Component)Activator.CreateInstance(type);
                            if (obj != null)
                            {
                                return fieldInfo.GetValue(obj).ToString();
                            }
                        }
                        catch { }
                    }
                }
            }

            return "";
        }

        private string GetComponentNameId(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (!fieldInfos.IsNullOrEmpty())
            {
                var constants = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
                if (!constants.IsNullOrEmpty())
                {
                    var fieldInfo = constants.FirstOrDefault(o => o.Name == "NameId");
                    if (fieldInfo != null)
                    {
                        try
                        {
                            var obj = (Component)Activator.CreateInstance(type);
                            if (obj != null)
                            {
                                return fieldInfo.GetValue(obj).ToString();
                            }
                        }
                        catch { }
                    }
                }
            }

            return "";
        }

        private string GetCompositionTypeId(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (!fieldInfos.IsNullOrEmpty())
            {
                var constants = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
                if (!constants.IsNullOrEmpty())
                {
                    var fieldInfo = constants.FirstOrDefault(o => o.Name == "TypeId");
                    if (fieldInfo != null)
                    {
                        try
                        {
                            var obj = (Composition)Activator.CreateInstance(type);
                            if (obj != null)
                            {
                                return fieldInfo.GetValue(obj).ToString();
                            }
                        }
                        catch { }
                    }
                }
            }

            return "";
        }

        private string GetCompositionNameId(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (!fieldInfos.IsNullOrEmpty())
            {
                var constants = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
                if (!constants.IsNullOrEmpty())
                {
                    var fieldInfo = constants.FirstOrDefault(o => o.Name == "NameId");
                    if (fieldInfo != null)
                    {
                        try
                        {
                            var obj = (Composition)Activator.CreateInstance(type);
                            if (obj != null)
                            {
                                return fieldInfo.GetValue(obj).ToString();
                            }
                        }
                        catch { }
                    }
                }
            }

            return "";
        }
    }
}