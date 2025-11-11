import argparse

from rfdetr.detr import RFDETRNano, RFDETRSmall, RFDETRMedium, RFDETRLarge


def get_model(model_type):
    model_mapping = {
        'nano': RFDETRNano,
        'small': RFDETRSmall,
        'medium': RFDETRMedium,
        'large': RFDETRLarge
    }
    if model_type not in model_type:
        raise ValueError(f"Model type '{model_type}' not supported. Available: {list(model_mapping.keys())}")
    return model_mapping[model_type]()

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--model', type=str, required=True,
                        help='Type of RF-DETR model to use',
                        choices=['nano', 'small', 'medium', 'large'])
    parser.add_argument('--dataset_dir', type=str, required=True,
                        help='Specifies the COCO-formatted dataset location with train, valid, and test folders, each containing _annotations.coco.json. Ensures the model can properly read and parse data.')
    parser.add_argument('--output_dir', type=str, required=True,
                        help='Directory where training artifacts (checkpoints, logs, etc.) are saved. Important for experiment tracking and resuming training.')

    parser.add_argument('--epochs', type=int, default=10,
                        help='Number of full passes over the dataset. Increasing this can improve performance but extends total training time.')
    parser.add_argument('--batch_size', type=int, default=4,
                        help='Number of samples processed per iteration. Higher values require more GPU memory but can speed up training. Must be balanced with grad_accum_steps to maintain the intended total batch size.')
    parser.add_argument('--grad_accum_steps', type=int, default=4,
                        help='Accumulates gradients over multiple mini-batches, effectively raising the total batch size without requiring as much memory at once. Helps train on smaller GPUs at the cost of slightly more time per update.')
    parser.add_argument('--lr', type=float, default=1e-4,
                        help='Learning rate for most parts of the model. Influences how quickly or cautiously the model adjusts its parameters.')
    parser.add_argument('--resolution', type=int, default=224,
                        help='Sets the input image dimensions. Higher values can improve accuracy but require more memory and can slow training. Must be divisible by 32.')

    args = parser.parse_args()
    
    model = get_model(args.model)
    
    model.train(
        dataset_dir=args.dataset_dir,
        epochs=args.epochs,
        batch_size=args.batch_size,
        grad_accum_steps=args.grad_accum_steps,
        lr=args.lr,
        output_dir=args.output_dir,
        resolution=args.resolution
    )

if __name__ == '__main__':
    main()