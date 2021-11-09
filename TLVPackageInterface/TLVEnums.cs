using System;

namespace TLVPackageInterface
{
    public enum TLVDestinationIDs : uint
    {
        TLV_DEST_ID_NOT_SET = 0,
        TLV_DEST_ID_PORTAL = 1,
        TLV_DEST_ID_RI = 2,
        TLV_DEST_ID_ALL = 0xFFFFFFFF
    }

    public enum TLVPackageTypes : byte
    {
        TLV_PKG_LOG = 0,
        TLV_PKG_COMMAND = 1,
        TLV_PKG_RESPONSE = 2,
        TLV_PKG_RTCM = 3,
        TLV_PKG_V2V = 4,
        TLV_PKG_ATTACHED_DEVICE = 5,
        TLV_PKG_HP_BUCKET = 6
    }

    public enum SYNC_STATUS
    {
        SYNC_STATUS_READY_TO_SEND = 1,
        SYNC_STATUS_SENDING = 2,
        SYNC_STATUS_SENDING_TIMEOUT = 4,
        SYNC_STATUS_RESPONSE_READY = 5,
        SYNC_STATUS_FAULTY = 6,
        SYNC_STATUS_WONT_BE_SENT_TO_PIN = 7,
        SYNC_STATUS_CANCELED = 8,
        SYNC_STATUS_ACK_RECEIVED = 9,
        SYNC_STATUS_RI_RECEIVED = 10,
    }
}
