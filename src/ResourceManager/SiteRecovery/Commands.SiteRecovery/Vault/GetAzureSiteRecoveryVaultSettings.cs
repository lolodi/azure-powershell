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
using System.Management.Automation;

namespace Microsoft.Azure.Commands.SiteRecovery
{
    /// <summary>
    /// Retrieves Azure Site Recovery Vault Settings.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureRmSiteRecoveryVaultSettings")]
    [OutputType(typeof(ASRVaultSettings))]
    [Obsolete("This cmdlet has been marked for deprecation in an upcoming release. Please use the " +
        "equivalent cmdlet from the AzureRm.RecoveryServices.SiteRecovery module instead.",
        false)]
    public class GetAzureSiteRecoveryVaultSettings : SiteRecoveryCmdletBase
    {
        /// <summary>
        /// ProcessRecord of the command.
        /// </summary>
        public override void ExecuteSiteRecoveryCmdlet()
        {
            base.ExecuteSiteRecoveryCmdlet();

            this.WriteObject(new ASRVaultSettings(PSRecoveryServicesClient.asrVaultCreds));
        }
    }
}