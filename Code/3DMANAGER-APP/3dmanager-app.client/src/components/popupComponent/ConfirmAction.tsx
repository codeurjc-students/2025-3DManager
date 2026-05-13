import ConfirmPopup from "./ConfirmPopup";
import InfoPopup from "./InfoPopup";
import type { CommonResponse } from "../../models/base/CommonResponse";
import type { PopupData } from "../../models/popup/PopupData";

export const confirmAction = ({
    action,
    service,
    successMessage,
    errorMessage,
    onSuccess,
    onError,
    showPopup,
    reopenGroupPopup
}: {
    action: string;
    service: () => Promise<CommonResponse<unknown>>;
    successMessage: string;
    errorMessage: string;
    onSuccess?: () => void | Promise<void>;
    onError?: () => void;
    showPopup: (props: PopupData) => void;
    reopenGroupPopup: () => void;
}) => {

    showPopup({
        type: "base",
        hideCloseButton: true,
        content: (
            <ConfirmPopup
                action={action}
                onCancel={reopenGroupPopup}
                onConfirm={async () => {
                    const response = await service();

                    if (response.data) {
                        showPopup({
                            type: "info",
                            content: (
                                <InfoPopup
                                    title="Operación realizada"
                                    description={successMessage}
                                />
                            ),
                            onClose: onSuccess ?? reopenGroupPopup
                        });
                    } else {
                        showPopup({
                            type: "error",
                            content: (
                                <InfoPopup
                                    title="Error"
                                    description={response.error?.message || errorMessage}
                                />
                            ),
                            onClose: onError ?? reopenGroupPopup
                        });
                    }
                }}
            />
        )
    });
};