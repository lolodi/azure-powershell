﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.SiteRecovery.Models;

namespace Microsoft.Azure.Commands.SiteRecovery
{
    /// <summary>
    /// Retrieves Azure Site Recovery storage classification.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureRmSiteRecoveryStorageClassification", DefaultParameterSetName = ASRParameterSets.Default)]
    [OutputType(typeof(IEnumerable<ASRStorageClassification>))]
    public class GetAzureSiteRecoveryStorageClassification : SiteRecoveryCmdletBase
    {
        #region Parameters
        /// <summary>
        /// Gets or sets name of classification.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByName, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets friendly name of classification.
        /// </summary>
        [Parameter(ParameterSetName = ASRParameterSets.ByFriendlyName, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string FriendlyName { get; set; }
        #endregion

        /// <summary>
        /// ProcessRecord of the command.
        /// </summary>
        public override void ExecuteSiteRecoveryCmdlet()
        {
            base.ExecuteSiteRecoveryCmdlet();

            List<Fabric> fabrics = new List<Fabric>();
            List<StorageClassification> storageClassifications = new List<StorageClassification>();

            Task fabricTask = RecoveryServicesClient.EnumerateFabricsAsync((entities) =>
                {
                    fabrics.AddRange(entities);
                });

            Task storageClassificationTask =
                RecoveryServicesClient.EnumerateStorageClassificationsAsync((entities) =>
                {
                    storageClassifications.AddRange(entities);
                });

            Task.WaitAll(fabricTask, storageClassificationTask);

            var fabricMap = fabrics.ToDictionary(item => item.Id, item => item);

            switch (this.ParameterSetName)
            {
                case ASRParameterSets.ByFriendlyName:
                    storageClassifications = storageClassifications.Where(item =>
                        item.Properties.FriendlyName.Equals(
                            this.FriendlyName, 
                            StringComparison.InvariantCultureIgnoreCase))
                        .ToList();
                    break;
                case ASRParameterSets.ByName:
                    storageClassifications = storageClassifications.Where(item =>
                        item.Name.Equals(
                            this.Name, 
                            StringComparison.InvariantCultureIgnoreCase))
                        .ToList();
                    break;
            }

            var psObject = storageClassifications.ConvertAll(item =>
                {
                    var fabric = fabricMap[item.GetFabricId()];

                    return new ASRStorageClassification()
                    {
                        FabricFriendlyName = fabric.Properties.FriendlyName,
                        FriendlyName = item.Properties.FriendlyName,
                        Id = item.Id,
                        Name = item.Name
                    };
                });

            this.WriteObject(psObject, true);
        }
    }
}
