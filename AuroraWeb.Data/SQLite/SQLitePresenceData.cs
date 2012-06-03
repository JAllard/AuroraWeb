/*
 * Copyright (c) Marcus Kirsch (aka Marck). Contributors, http://aurora-sim.org/, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using Aurora.Framework;
using OpenMetaverse;
using OpenSim.Services.Interfaces;

namespace AuroraWeb.Data.SQLite
{
    /// <summary>
    /// A MySQL Interface for the Grid Server
    /// </summary>
    public class SQLitePresenceData : IScenePresence
    {
        protected Assembly Assembly
        {
            get { return GetType().BaseType.Assembly; }
        }

        public SQLitePresenceData(string connectionString, string realm)
            
        {
        }

        public UUID UUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public uint LocalId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int LinkNum
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 AbsolutePosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 Velocity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Quaternion Rotation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void RegisterModuleInterface<T>(T mod)
        {
            throw new NotImplementedException();
        }

        public void AddModuleInterfaces(Dictionary<Type, List<object>> dictionary)
        {
            throw new NotImplementedException();
        }

        public void UnregisterModuleInterface<T>(T mod)
        {
            throw new NotImplementedException();
        }

        public void StackModuleInterface<T>(T mod)
        {
            throw new NotImplementedException();
        }

        public T RequestModuleInterface<T>()
        {
            throw new NotImplementedException();
        }

        public bool TryRequestModuleInterface<T>(out T iface)
        {
            throw new NotImplementedException();
        }

        public T[] RequestModuleInterfaces<T>()
        {
            throw new NotImplementedException();
        }

        public Dictionary<Type, List<object>> GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public void RemoveAllInterfaces()
        {
            throw new NotImplementedException();
        }

        public event AddPhysics OnAddPhysics;
        public event RemovePhysics OnRemovePhysics;
        public event AddPhysics OnSignificantClientMovement;

        public IScene Scene
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string CallbackURI
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Firstname
        {
            get { throw new NotImplementedException(); }
        }

        public string Lastname
        {
            get { throw new NotImplementedException(); }
        }

        public IClientAPI ControllingClient
        {
            get { throw new NotImplementedException(); }
        }

        public ISceneViewer SceneViewer
        {
            get { throw new NotImplementedException(); }
        }

        public IAnimator Animator
        {
            get { throw new NotImplementedException(); }
        }

        public PhysicsCharacter PhysicsActor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsChildAgent
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ulong RootAgentHandle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 Lookat
        {
            get { throw new NotImplementedException(); }
        }

        public Vector3 CameraPosition
        {
            get { throw new NotImplementedException(); }
        }

        public Quaternion CameraRotation
        {
            get { throw new NotImplementedException(); }
        }

        public Vector3 OffsetPosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public UUID ParentID
        {
            get { throw new NotImplementedException(); }
        }

        public bool AllowMovement
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool FallenStandUp
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool FlyDisabled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool ForceFly
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int GodLevel
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool SetAlwaysRun
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsBusy
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public byte State
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public uint AgentControlFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public float SpeedModifier
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector4 CollisionPlane
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Frozen
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int UserLevel
        {
            get { throw new NotImplementedException(); }
        }

        public void Teleport(Vector3 Pos)
        {
            throw new NotImplementedException();
        }

        public Vector3 LastKnownAllowedPosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public UUID CurrentParcelUUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ILandObject CurrentParcel
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Invulnerable
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public float DrawDistance
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 CameraAtAxis
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDeleted
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool SitGround
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsInTransit
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 ParentPosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void PushForce(Vector3 impulse)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void ChildAgentDataUpdate(AgentData agentData)
        {
            throw new NotImplementedException();
        }

        public void ChildAgentDataUpdate(AgentPosition cAgentData, int regionX, int regionY, int globalX, int globalY)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(AgentData agent)
        {
            throw new NotImplementedException();
        }

        public void MakeRootAgent(Vector3 pos, bool isFlying, bool makePhysicalActor)
        {
            throw new NotImplementedException();
        }

        public void MakeChildAgent(GridRegion destindation)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void DoAutoPilot(uint objectLocalID, Vector3 pos, IClientAPI avatar)
        {
            throw new NotImplementedException();
        }

        public void TeleportWithMomentum(Vector3 value)
        {
            throw new NotImplementedException();
        }

        public void DoMoveToPosition(object iClientAPI, string p, List<string> coords)
        {
            throw new NotImplementedException();
        }

        public void SuccessfulTransit()
        {
            throw new NotImplementedException();
        }

        public void SuccessfulCrossingTransit(GridRegion CrossingRegion)
        {
            throw new NotImplementedException();
        }

        public void FailedTransit()
        {
            throw new NotImplementedException();
        }

        public void FailedCrossingTransit(GridRegion failedCrossingRegion)
        {
            throw new NotImplementedException();
        }

        public void AddNewMovement(Vector3 force, Quaternion quaternion)
        {
            throw new NotImplementedException();
        }

        public void AddToPhysicalScene(bool m_flying, bool p)
        {
            throw new NotImplementedException();
        }

        public void SendTerseUpdateToAllClients()
        {
            throw new NotImplementedException();
        }

        public void SendCoarseLocations(List<Vector3> coarseLocations, List<UUID> avatarUUIDs)
        {
            throw new NotImplementedException();
        }

        public void AddUpdateToAvatar(ISceneChildEntity entity, PrimUpdateFlags PostUpdateFlags)
        {
            throw new NotImplementedException();
        }

        public void StandUp()
        {
            throw new NotImplementedException();
        }

        public void SetHeight(float height)
        {
            throw new NotImplementedException();
        }

        public UUID SittingOnUUID
        {
            get { throw new NotImplementedException(); }
        }

        public bool Sitting
        {
            get { throw new NotImplementedException(); }
        }

        public void ClearSavedVelocity()
        {
            throw new NotImplementedException();
        }

        public void HandleAgentRequestSit(IClientAPI remoteClient, UUID targetID, Vector3 offset)
        {
            throw new NotImplementedException();
        }

        public bool IsTainted
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public PresenceTaint Taints
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector3 GetAbsolutePosition()
        {
            throw new NotImplementedException();
        }

        public void AddChildAgentUpdateTaint(int seconds)
        {
            throw new NotImplementedException();
        }

        public void SetAttachments(ISceneEntity[] groups)
        {
            throw new NotImplementedException();
        }

        public void TriggerSignificantClientMovement()
        {
            throw new NotImplementedException();
        }

        public void SetAgentLeaving(GridRegion destindation)
        {
            throw new NotImplementedException();
        }

        public void AgentFailedToLeave()
        {
            throw new NotImplementedException();
        }

        public bool SuccessfullyMadeRootAgent
        {
            get { throw new NotImplementedException(); }
        }

        public bool AttachmentsLoaded
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}