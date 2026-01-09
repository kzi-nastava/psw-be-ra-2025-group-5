using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain
{
    public class KeyPointChallenge : Entity
    {
        public long KeyPointId { get; private set; }
        public long ChallengeId { get; private set; }
        public bool IsRequiredForSecret { get; private set; }
        public bool IsRequiredForCompletion { get; private set; }

        public KeyPointChallenge(long keyPointId, long challengeId, bool isRequiredForSecret, bool isRequiredForCompletion)
        {
            KeyPointId = keyPointId;
            ChallengeId = challengeId;
            IsRequiredForSecret = isRequiredForSecret;
            IsRequiredForCompletion = isRequiredForCompletion;
        }

        public KeyPointChallenge()
        {

        }
    }
}