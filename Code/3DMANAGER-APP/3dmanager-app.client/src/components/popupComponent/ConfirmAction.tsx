import ConfirmPopup from "./ConfirmPopup";
import InfoPopup from "./InfoPopup";

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
    service: () => Promise<any>;
    successMessage: string;
    errorMessage: string;
    onSuccess?: () => void;
    onError?: () => void;
    showPopup: any;
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
