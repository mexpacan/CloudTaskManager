<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CloudTaskManger" generation="1" functional="0" release="0" Id="6191a517-ae1b-4c97-be0c-bf277b422c9f" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CloudTaskMangerGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="TMWebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CloudTaskManger/CloudTaskMangerGroup/LB:TMWebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="TMWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudTaskManger/CloudTaskMangerGroup/MapTMWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="TMWebRole:StorageConnection" defaultValue="">
          <maps>
            <mapMoniker name="/CloudTaskManger/CloudTaskMangerGroup/MapTMWebRole:StorageConnection" />
          </maps>
        </aCS>
        <aCS name="TMWebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudTaskManger/CloudTaskMangerGroup/MapTMWebRoleInstances" />
          </maps>
        </aCS>
        <aCS name="TMWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudTaskManger/CloudTaskMangerGroup/MapTMWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="TMWorkerRole:StorageConnection" defaultValue="">
          <maps>
            <mapMoniker name="/CloudTaskManger/CloudTaskMangerGroup/MapTMWorkerRole:StorageConnection" />
          </maps>
        </aCS>
        <aCS name="TMWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudTaskManger/CloudTaskMangerGroup/MapTMWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:TMWebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapTMWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapTMWebRole:StorageConnection" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRole/StorageConnection" />
          </setting>
        </map>
        <map name="MapTMWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRoleInstances" />
          </setting>
        </map>
        <map name="MapTMWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWorkerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapTMWorkerRole:StorageConnection" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWorkerRole/StorageConnection" />
          </setting>
        </map>
        <map name="MapTMWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="TMWebRole" generation="1" functional="0" release="0" software="C:\Users\pablocano\OneDrive\HKR\Cloud\Project\v03\CloudTaskManger\CloudTaskManger\csx\Release\roles\TMWebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnection" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;TMWebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;TMWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;TMWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="TMWorkerRole" generation="1" functional="0" release="0" software="C:\Users\pablocano\OneDrive\HKR\Cloud\Project\v03\CloudTaskManger\CloudTaskManger\csx\Release\roles\TMWorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnection" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;TMWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;TMWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;TMWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="TMWebRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="TMWorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="TMWebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="TMWorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="TMWebRoleInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="TMWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="9be5f798-9c39-44b8-9da9-034e8d684806" ref="Microsoft.RedDog.Contract\ServiceContract\CloudTaskMangerContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="30d775ae-436f-419a-bc86-6edafb42d0bb" ref="Microsoft.RedDog.Contract\Interface\TMWebRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CloudTaskManger/CloudTaskMangerGroup/TMWebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>